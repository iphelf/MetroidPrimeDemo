﻿using MetroidPrimeDemo.Scripts.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player.Abilities
{
    public class FireBeamAbility : SimpleAttackAbility
    {
        private InputAction _input;
        [SerializeField] private float cooldown = 0.18f;
        [SerializeField] private int maxSuccession = 3;
        private bool _wasFiringBeam;
        private int _succession;
        private float _lastFireTime = float.MinValue;

        public override void Initialize(InputConfig inputConfig, AbilityConfig abilityConfig)
        {
            base.Initialize(inputConfig, abilityConfig);
            _input = inputConfig.data.ActionsAsset.FindAction(inputConfig.data.action);
            player.cannon.regularBeam.OnDamage.AddListener(OnDamage);
        }

        private void OnDestroy()
        {
            player.cannon.regularBeam.OnDamage.RemoveListener(OnDamage);
        }

        private bool FiringBeam() => _input.IsPressed();

        private void Update()
        {
            bool isFiringBeam = FiringBeam();

            if (isFiringBeam)
            {
                if (!_wasFiringBeam)
                    _succession = 0;

                if (_succession < maxSuccession && Time.time > _lastFireTime + cooldown)
                {
                    player.cannon.Fire();
                    _lastFireTime = Time.time;
                    ++_succession;
                }
            }

            _wasFiringBeam = isFiringBeam;
        }
    }
}