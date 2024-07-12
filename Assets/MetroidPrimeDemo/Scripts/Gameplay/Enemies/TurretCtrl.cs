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

        private StateMachine<State, Trigger> _fsm;

        private enum State
        {
            Idle,
            PowerOn,
            Attack,
            PowerOff,
            Dying,
        }

        private enum Trigger
        {
            Damaged,
        }

        protected override void Start()
        {
            base.Start();

            _vision = GetComponent<EnemyVision>();
            _vision.SetTarget(Player.transform);

            gun.beam.OnDamage.AddListener(OnDamage);

            _fsm = new StateMachine<State, Trigger>();

            _fsm.AddState(State.Idle);
            _fsm.AddState(State.PowerOn, new CoState<State>(this, PowerOnRoutine, needsExitTime: true));
            _fsm.AddState(State.Attack, new CoState<State>(this, AttackRoutine));
            _fsm.AddState(State.PowerOff, new CoState<State>(this, PowerOffRoutine, needsExitTime: true));
            _fsm.AddState(State.Dying, new CoState<State>(this, DieRoutine));
            _fsm.SetStartState(State.Idle);

            _fsm.AddTransition(State.Idle, State.PowerOn, _ => _vision.CanSee());
            _fsm.AddTransition(State.PowerOn, State.Attack);
            _fsm.AddTransition(State.Attack, State.PowerOff, _ => _vision.LastTimeSeen + 4.0f < Time.time);
            _fsm.AddTransition(State.PowerOff, State.Idle);
            _fsm.AddTriggerTransitionFromAny(Trigger.Damaged, State.Dying, forceInstantly: true);

            _fsm.Init();
        }

        private void Update()
        {
            _fsm.OnLogic();
        }

        private void LateUpdate()
        {
            if (_fsm.ActiveStateName != State.Attack) return;
            Quaternion currentRotation = gun.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(_vision.LastPositionSeen - gun.transform.position);
            currentRotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            currentRotation = GeometryHelpers.ClampEulerPitch(currentRotation, pitchRange.x, pitchRange.y);
            gun.transform.rotation = currentRotation;
        }

        private IEnumerator PowerOnRoutine()
        {
            yield return new WaitForSeconds(1.0f);
            _fsm.StateCanExit();
        }

        private IEnumerator AttackRoutine()
        {
            while (_fsm.ActiveStateName == State.Attack)
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
            _fsm.StateCanExit();
        }

        private void OnDamage(GameObject other)
        {
            var player = other.GetComponent<PlayerCharacterCtrl>();
            if (player is null) return;
            player.Hurt(damage);
        }

        protected override void OnDamaged()
        {
            _fsm.Trigger(Trigger.Damaged);
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