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
            _input = inputConfig.data.ActionsAsset.FindAction(inputConfig.data.action);
            if (abilityConfig.data.TryReadConfig(nameof(speed), out var speedString))
                float.TryParse(speedString, out speed);
        }

        private bool Jumped() => _input.WasPressedThisFrame();

        public void Update()
        {
            if (!Attributes.WasGrounded) return;
            if (!Jumped()) return;
            Attributes.Velocity.y += speed;
            Attributes.LastJumpTime = Time.time;
            Attributes.WasGrounded = false;
        }
    }
}