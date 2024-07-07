using System.Collections;
using MetroidPrimeDemo.Scripts.Gameplay.EnemyAI;
using MetroidPrimeDemo.Scripts.Gameplay.Weapons;
using MetroidPrimeDemo.Scripts.General;
using NaughtyAttributes;
using UnityEngine;
using UnityHFSM;

namespace MetroidPrimeDemo.Scripts.Gameplay.Enemies
{
    [RequireComponent(typeof(EnemyVision))]
    public class TurretCtrl : EnemyCharacterCtrl
    {
        [SerializeField] private GameObject explosionObject;
        [SerializeField] private TurretGunCtrl gun;
        [SerializeField] private float damage = 25.0f;
        [SerializeField] private float cooldown = 0.18f;
        [SerializeField] private int maxSuccession = 2;
        [SerializeField] private float successionCooldown = 0.8f;

        [SerializeField, MinMaxSlider(-89.0f, 89.0f)]
        private Vector2 pitchRange = new(-89.0f, 25.0f);

        [SerializeField] private float rotationSpeed = 25.0f;

        private EnemyVision _vision;

        private StateMachine _fsm;

        private static class States
        {
            public const string Idle = nameof(Idle);
            public const string PowerOn = nameof(PowerOn);
            public const string Attack = nameof(Attack);
            public const string PowerOff = nameof(PowerOff);
            public const string Dying = nameof(Dying);
        }

        private static class Triggers
        {
            public const string PoweredOn = nameof(PoweredOn);
            public const string PoweredOff = nameof(PoweredOff);
            public const string Damaged = nameof(Damaged);
        }


        protected override void Start()
        {
            base.Start();

            _vision = GetComponent<EnemyVision>();
            _vision.SetTarget(Player.transform);

            gun.beam.OnDamage.AddListener(OnDamage);

            _fsm = new StateMachine();

            _fsm.AddState(States.Idle);
            _fsm.AddState(States.PowerOn, new CoState(this, PowerOnRoutine));
            _fsm.AddState(States.Attack, new CoState(this, AttackRoutine));
            _fsm.AddState(States.PowerOff, new CoState(this, PowerOffRoutine));
            _fsm.AddState(States.Dying, new CoState(this, DieRoutine));
            _fsm.SetStartState(States.Idle);

            _fsm.AddTransition(States.Idle, States.PowerOn, _ => _vision.CanSee());
            _fsm.AddTriggerTransition(Triggers.PoweredOn, States.PowerOn, States.Attack);
            _fsm.AddTransition(States.Attack, States.PowerOff, _ => _vision.LastTimeSeen + 4.0f < Time.time);
            _fsm.AddTriggerTransition(Triggers.PoweredOff, States.PowerOff, States.Idle);
            _fsm.AddTriggerTransitionFromAny(Triggers.Damaged, States.Dying);

            _fsm.Init();
        }

        private void Update()
        {
            _fsm.OnLogic();
        }

        private void LateUpdate()
        {
            if (_fsm.ActiveStateName != States.Attack) return;
            Quaternion currentRotation = gun.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(_vision.LastPositionSeen - gun.transform.position);
            currentRotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            currentRotation = GeometryHelpers.ClampEulerPitch(currentRotation, pitchRange.x, pitchRange.y);
            gun.transform.rotation = currentRotation;
        }

        private IEnumerator PowerOnRoutine()
        {
            yield return new WaitForSeconds(1.0f);
            _fsm.Trigger(Triggers.PoweredOn);
        }

        private IEnumerator AttackRoutine()
        {
            while (_fsm.ActiveStateName == States.Attack)
            {
                for (int i = 0; i < maxSuccession; ++i)
                {
                    gun.Fire();
                    yield return new WaitForSeconds(cooldown);
                }

                yield return new WaitForSeconds(successionCooldown);
            }
        }

        private IEnumerator PowerOffRoutine()
        {
            yield return new WaitForSeconds(1.0f);
            _fsm.Trigger(Triggers.PoweredOff);
        }

        private void OnDamage(GameObject other)
        {
            var player = other.GetComponent<PlayerCharacterCtrl>();
            if (player is null) return;
            player.Hurt(damage);
        }

        protected override void OnDamaged()
        {
            _fsm.Trigger(Triggers.Damaged);
        }

        private IEnumerator DieRoutine()
        {
            explosionObject.SetActive(true);
            OnDisable();
            yield return new WaitForSeconds(1.0f);
            Destroy(gameObject);
        }
    }
}