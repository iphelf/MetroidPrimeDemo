using System;
using System.Collections;
using MetroidPrimeDemo.Scripts.Gameplay.EnemyAI;
using MetroidPrimeDemo.Scripts.Gameplay.Weapons;
using MetroidPrimeDemo.Scripts.General;
using NaughtyAttributes;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Enemies
{
    [SelectionBase]
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
        private Awaitable _ai;
        private bool _isAttacking;

        private async void Start()
        {
            _vision = GetComponent<EnemyVision>();
            gun.beam.OnDamage.AddListener(OnDamage);

            try
            {
                _ai = AIAsync();
                await _ai;
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void OnDestroy()
        {
            gun.beam.OnDamage.RemoveListener(OnDamage);
            _ai?.Cancel();
        }

        protected override void OnDamaged()
        {
            StartCoroutine(DestroyRoutine());
        }

        private IEnumerator DestroyRoutine()
        {
            _ai.Cancel();
            _ai = null;
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

        private async Awaitable AIAsync()
        {
            while (enabled)
            {
                await WaitUntilPlayerShowsUpAsync();
                await NotifyPlayerAttackingAsync();
                _isAttacking = true;
                Awaitable attacking = AttackAsync();
                await WaitUntilPlayerDisappearsAsync();
                _isAttacking = false;
                await attacking;
                await NotifyPlayerAttackOverAsync();
            }
        }

        private async Awaitable WaitUntilPlayerShowsUpAsync() => await WaitForPlayerAsync(float.MaxValue);

        private async Awaitable<bool> WaitForPlayerAsync(float maxDuration)
        {
            float startTime = Time.time;
            while (true)
            {
                bool canSee = _vision.CanSee(Player.transform.position);
                if (canSee)
                    return true;
                await Awaitable.WaitForSecondsAsync(0.2f);
                if (Time.time - startTime > maxDuration)
                    return false;
            }
        }

        private async Awaitable NotifyPlayerAttackingAsync()
        {
            await Awaitable.WaitForSecondsAsync(1.0f);
        }

        private async Awaitable NotifyPlayerAttackOverAsync()
        {
            await Awaitable.WaitForSecondsAsync(1.0f);
        }

        private async Awaitable AttackAsync()
        {
            while (_isAttacking)
            {
                for (int i = 0; i < maxSuccession; ++i)
                {
                    gun.Fire();
                    await Awaitable.WaitForSecondsAsync(cooldown);
                }

                await Awaitable.WaitForSecondsAsync(successionCooldown);
            }
        }

        private void OnDamage(GameObject other)
        {
            var player = other.GetComponent<PlayerCharacterCtrl>();
            if (player is null) return;
            player.Hurt(damage);
        }

        private async Awaitable WaitUntilPlayerDisappearsAsync()
        {
            while (true)
            {
                bool canSee = _vision.CanSee(Player.transform.position);
                if (!canSee)
                    canSee = await WaitForPlayerAsync(4.0f);
                if (!canSee)
                    break;
                await Awaitable.WaitForSecondsAsync(0.2f);
            }
        }
    }
}