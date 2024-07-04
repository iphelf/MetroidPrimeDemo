using MetroidPrimeDemo.Scripts.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player.Abilities
{
    public class JumpAbility : Ability
    {
        [SerializeField] private float speed = 9.0f;

        private InputAction _input;

        public override void Initialize(InputConfig inputConfig, AbilityConfig abilityConfig)
        {
            base.Initialize(inputConfig, abilityConfig);
            _input = inputConfig.data.ActionsAsset.FindAction(inputConfig.data.action);
        }

        private bool Jumped() => _input.WasPressedThisFrame();

        public void Update()
        {
            if (!attributes.wasGrounded) return;
            if (!Jumped()) return;
            attributes.velocity.y += speed;
            attributes.lastJumpTime = Time.time;
            attributes.wasGrounded = false;
        }
    }
}