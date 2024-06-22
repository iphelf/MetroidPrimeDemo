using MetroidPrimeDemo.Scripts.Data;
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
            _stickInput = inputConfig.data.ActionsAsset.FindAction(inputConfig.data.action);
            if (inputConfig.data.TryReadConfig("pointerAction", out var pointerActionString))
                _pointerInput = inputConfig.data.ActionsAsset.FindAction(pointerActionString);
            if (abilityConfig.data.TryReadConfig(nameof(gamepadRotationSpeed), out var gamepadRotationSpeedString))
                float.TryParse(gamepadRotationSpeedString, out gamepadRotationSpeed);
            if (abilityConfig.data.TryReadConfig(nameof(mouseSensitivity), out var mouseSensitivityString))
                float.TryParse(mouseSensitivityString, out mouseSensitivity);
        }

        private Vector2 LookDelta()
        {
            var look = _stickInput.ReadValue<Vector2>();
            // Check if this look input is coming from the mouse
            if (look.Equals(Vector2.zero))
                look = _pointerInput.ReadValue<Vector2>() * mouseSensitivity;
            // or from the gamepad
            else
                look *= Time.deltaTime * gamepadRotationSpeed;

            look.y = -look.y;

            return look;
        }

        private void Update()
        {
            Vector2 lookDelta = LookDelta();

            _yaw += lookDelta.x;
            _yaw %= 360.0f;

            _pitch += lookDelta.y;
            _pitch = Mathf.Clamp(_pitch, -89f, 89f);

            player.SetRotation(_yaw, _pitch);
        }
    }
}