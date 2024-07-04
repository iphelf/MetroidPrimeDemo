using MetroidPrimeDemo.Scripts.Data;
using MetroidPrimeDemo.Scripts.General;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player.Abilities
{
    public class LookAbility : Ability
    {
        [SerializeField] private float gamepadRotationSpeed = 150.0f;
        [SerializeField] private float mouseSensitivity = 0.08f;

        private InputAction _stickInput;
        private InputAction _pointerInput;

        private float _yaw;
        private float _pitch;

        public override void Initialize(InputConfig inputConfig, AbilityConfig abilityConfig)
        {
            base.Initialize(inputConfig, abilityConfig);
            _stickInput = inputConfig.data.ActionsAsset.FindAction(inputConfig.data.action);
            if (inputConfig.data.TryReadConfig("pointerAction", out var pointerActionString))
                _pointerInput = inputConfig.data.ActionsAsset.FindAction(pointerActionString);
        }

        private Vector2 LookDelta()
        {
            var look = _stickInput.ReadValue<Vector2>();
            // Check if this look input is coming from the mouse
            if (look.Equals(Vector2.zero))
                look = Time.timeScale * mouseSensitivity * _pointerInput.ReadValue<Vector2>();
            // or from the gamepad
            else
                look *= Time.deltaTime * gamepadRotationSpeed;

            look.y = -look.y;

            return look;
        }

        private void Update()
        {
            player.GetRotation(out _pitch, out _yaw);
            if (attributes.lockTarget is not null)
                return;

            Vector2 lookDelta = LookDelta();
            _yaw += lookDelta.x;
            _pitch += lookDelta.y;

            GeometryHelpers.Normalize(ref _pitch, ref _yaw);
            player.SetRotation(_pitch, _yaw);
        }
    }
}