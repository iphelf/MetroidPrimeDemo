using System;
using System.Collections;
using System.Threading;
using MetroidPrimeDemo.Scripts.Gameplay.EnemyAI;
using MetroidPrimeDemo.Scripts.Gameplay.Weapons;
using MetroidPrimeDemo.Scripts.General;
using NaughtyAttributes;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Enemies
{
    [RequireComponent(typeof(EnemyVision))]
    public class TurretCtrl : EnemyCharacterCtrl
    {
        [SerializeField] private TurretGunCtrl gun;
        [SerializeField] private float damage = 20.0f;
        [SerializeField] private float cooldown = 0.18f;
        [SerializeField] private int maxSuccession = 2;
        [SerializeField] private float successionCooldown = 0.8f;

        [SerializeField, MinMaxSlider(-89.0f, 89.0f)]
        private Vector2 pitchRange = new(-89.0f, 25.0f);

        [SerializeField] private float rotationSpeed = 25.0f;
        [SerializeField] private GameObject explosionObject;

        private EnemyVision _vision;
        private readonly CancellationTokenSource _cancelAi = new();
        private bool _isAttacking;

        protected override async void Start()
        {
            base.Start();

            _vision = GetComponent<EnemyVision>();
            gun.beam.OnDamage.AddListener(OnDamage);

            try
            {
                await AIAsync(_cancelAi.Token);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void OnDestroy()
        {
            gun.beam.OnDamage.RemoveListener(OnDamage);
            _cancelAi.Cancel();
        }

        protected override void OnDamaged()
        {
            StartCoroutine(DestroyRoutine());
        }

        private IEnumerator DestroyRoutine()
        {
            _cancelAi.Cancel();
            _isAttacking = false;
            explosionObject.SetActive(true);
            OnDisable();
            yield return new WaitForSeconds(1.0f);
            Destroy(gameObject);
        }

        private void LateUpdate()
        {
            if (!_isAttacking) return;
            Quaternion currentRotation = gun.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(Player.transform.position - gun.transform.position);
            currentRotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            currentRotation = GeometryHelpers.ClampEulerPitch(currentRotation, pitchRange.x, pitchRange.y);
            gun.transform.rotation = currentRotation;
        }

        private async Awaitable AIAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await WaitUntilPlayerShowsUpAsync(cancellationToken);
                await NotifyPlayerAttackingAsync(cancellationToken);
                _isAttacking = true;
                Awaitable attacking = AttackAsync(cancellationToken);
                await WaitUntilPlayerDisappearsAsync(cancellationToken);
                _isAttacking = false;
                await attacking;
                await NotifyPlayerAttackOverAsync(cancellationToken);
            }
        }

        private async Awaitable WaitUntilPlayerShowsUpAsync(CancellationToken cancellationToken) =>
            await WaitForPlayerAsync(float.MaxValue, cancellationToken);

        private async Awaitable<bool> WaitForPlayerAsync(float maxDuration, CancellationToken cancellationToken)
        {
            float startTime = Time.time;
            while (true)
            {
                bool canSee = _vision.CanSee(Player.transform.position);
                if (canSee)
                    return true;
                await Awaitable.WaitForSecondsAsync(0.2f, cancellationToken);
                if (Time.time - startTime > maxDuration)
                    return false;
            }
        }

        private async Awaitable NotifyPlayerAttackingAsync(CancellationToken cancellationToken)
        {
            await Awaitable.WaitForSecondsAsync(1.0f, cancellationToken);
        }

        private async Awaitable NotifyPlayerAttackOverAsync(CancellationToken cancellationToken)
        {
            await Awaitable.WaitForSecondsAsync(1.0f, cancellationToken);
        }

        private async Awaitable AttackAsync(CancellationToken cancellationToken)
        {
            while (_isAttacking)
            {
                for (int i = 0; i < maxSuccession; ++i)
                {
                    gun.Fire();
                    await Awaitable.WaitForSecondsAsync(cooldown, cancellationToken);
                }

                await Awaitable.WaitForSecondsAsync(successionCooldown, cancellationToken);
            }
        }

        private void OnDamage(GameObject other)
        {
            var player = other.GetComponent<PlayerCharacterCtrl>();
            if (player is null) return;
            player.Hurt(damage);
        }

        private async Awaitable WaitUntilPlayerDisappearsAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                bool canSee = _vision.CanSee(Player.transform.position);
                if (!canSee)
                    canSee = await WaitForPlayerAsync(4.0f, cancellationToken);
                if (!canSee)
                    break;
                await Awaitable.WaitForSecondsAsync(0.2f, cancellationToken);
            }
        }
    }
}