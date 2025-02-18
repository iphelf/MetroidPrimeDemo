﻿using MetroidPrimeDemo.Scripts.Data;
using MetroidPrimeDemo.Scripts.Gameplay.Pickups;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player.Abilities
{
    public class ChargeBeamAbility : Ability
    {
        [SerializeField] private float delay = 1.0f;
        [SerializeField] private float partialDuration = 0.2f;
        [SerializeField] private float fullDuration = 0.5f;
        [SerializeField] private float underChargeDamage = 10.0f;
        [SerializeField] private float partialChargeDamage = 35.0f;
        [SerializeField] private float fullChargeDamage = 50.0f;
        [SerializeField] private float attractionRange = 15.0f;

        private InputAction _input;

        private bool _pressing;
        private float _pressBeginTime = float.MaxValue;
        private bool _charging;
        private float _chargeBeginTime = float.MaxValue;
        private bool _partiallyCharged;
        private bool _fullyCharged;

        public override void Initialize(InputConfig inputConfig, AbilityConfig abilityConfig)
        {
            base.Initialize(inputConfig, abilityConfig);
            _input = inputConfig.data.ActionsAsset.FindAction(inputConfig.data.action);

            player.cannon.underChargeBeam.OnDamage.AddListener(OnUnderChargeDamage);
            player.cannon.partiallyChargedBeam.OnDamage.AddListener(OnPartialChargeDamage);
            player.cannon.fullyChargedBeam.OnDamage.AddListener(OnFullChargeDamage);
        }

        private void OnDestroy()
        {
            player.cannon.underChargeBeam.OnDamage.RemoveListener(OnUnderChargeDamage);
            player.cannon.partiallyChargedBeam.OnDamage.RemoveListener(OnPartialChargeDamage);
            player.cannon.fullyChargedBeam.OnDamage.RemoveListener(OnFullChargeDamage);
        }

        private void OnUnderChargeDamage(GameObject other) => OnDamage(other, underChargeDamage);
        private void OnPartialChargeDamage(GameObject other) => OnDamage(other, partialChargeDamage);
        private void OnFullChargeDamage(GameObject other) => OnDamage(other, fullChargeDamage);

        private void OnDamage(GameObject other, float damage)
        {
            if (!Aimable.IsValid(other, out var aimable))
                return;
            if (aimable is EnemyCharacterCtrl enemy)
                enemy.DealDamage(damage);
            else
                Destroy(other.gameObject);
        }

        private void Update()
        {
            float time = Time.time;
            bool isPressed = _input.IsPressed();

            if (!_pressing && isPressed)
            {
                _pressing = true;
                _pressBeginTime = time;
            }

            if (_pressing && isPressed)
            {
                if (!_charging && time - _pressBeginTime > delay)
                {
                    _charging = true;
                    _chargeBeginTime = time;
                    player.cannon.StartCharging();
                }

                if (!_partiallyCharged && time - _chargeBeginTime > partialDuration)
                {
                    _partiallyCharged = true;
                }

                if (!_fullyCharged && time - _chargeBeginTime > fullDuration)
                {
                    player.cannon.StopCharging(punchBack: false);
                    _fullyCharged = true;
                    player.cannon.StartLoopingCharged();
                }
            }

            if (_pressing && !isPressed)
            {
                if (_fullyCharged)
                {
                    player.cannon.StopLoopingCharged();
                    player.cannon.FireFullyCharged();
                }
                else if (_partiallyCharged)
                {
                    player.cannon.StopCharging();
                    player.cannon.FirePartiallyCharged();
                }
                else if (_charging)
                {
                    player.cannon.StopCharging();
                    player.cannon.FireUnderCharge();
                }

                _pressing = false;
                _pressBeginTime = float.MaxValue;
                _charging = false;
                _chargeBeginTime = float.MaxValue;
                _fullyCharged = false;
            }

            if (_charging)
            {
                float sqrAttractionRange = attractionRange * attractionRange;
                foreach (var go in GameObject.FindGameObjectsWithTag("FloatingPickup"))
                {
                    if (Vector3.SqrMagnitude(go.transform.position - player.transform.position)
                        > sqrAttractionRange)
                        continue;
                    var pickup = go.GetComponent<FloatingPickup>();
                    pickup.MoveTowards(player.transform.position);
                }
            }
        }
    }
}