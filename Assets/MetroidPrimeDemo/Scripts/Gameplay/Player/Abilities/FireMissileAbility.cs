﻿using MetroidPrimeDemo.Scripts.Data;
using MetroidPrimeDemo.Scripts.Modules;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player.Abilities
{
    public class FireMissileAbility : Ability
    {
        private InputAction _input;
        [SerializeField] private float cooldown = 1.0f;
        [SerializeField] private float lifetime = 5.0f;
        [SerializeField] private GameObject missilePrefab;
        private float _lastLaunchTime = float.MinValue;

        public override void Initialize(InputConfig inputConfig, AbilityConfig abilityConfig)
        {
            _input = inputConfig.data.ActionsAsset.FindAction(inputConfig.data.action);
            if (!abilityConfig.data.TryReadDependency(nameof(missilePrefab), out missilePrefab))
                missilePrefab = null;
        }

        private void Update()
        {
            if (!_input.WasPressedThisFrame()) return;
            if (Time.time < _lastLaunchTime + cooldown) return;
            if (attributes.missiles < 1) return;

            --attributes.missiles;

            var missile = DynamicRoot.Instantiate(missilePrefab, player.cannon.missileSlot);
            var ctrl = missile.GetComponent<MissileCtrl>();
            ctrl.Launch(attributes.lockTarget, lifetime);
            player.cannon.Recoil(2.0f);
            _lastLaunchTime = Time.time;
        }
    }
}