using MetroidPrimeDemo.Scripts.Data;
using MetroidPrimeDemo.Scripts.Modules;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player.Abilities
{
    public class FireMissileAbility : SimpleAttackAbility
    {
        private InputAction _input;
        [SerializeField] private float cooldown = 1.0f;
        [SerializeField] private float lifetime = 5.0f;
        [SerializeField] private GameObject missilePrefab;
        private float _lastFireTime = float.MinValue;

        public override void Initialize(InputConfig inputConfig, AbilityConfig abilityConfig)
        {
            base.Initialize(inputConfig, abilityConfig);
            _input = inputConfig.data.ActionsAsset.FindAction(inputConfig.data.action);
        }

        private void Update()
        {
            if (!_input.WasPressedThisFrame()) return;
            if (Time.time < _lastFireTime + cooldown) return;
            if (attributes.missiles < 1) return;

            --attributes.missiles;

            var missile = DynamicRoot.Instantiate(missilePrefab, player.cannon.missileSlot);
            var ctrl = missile.GetComponent<MissileCtrl>();
            ctrl.Launch(attributes.lockTarget, lifetime, OnDamage);
            player.cannon.Recoil(2.0f);
            _lastFireTime = Time.time;
        }
    }
}