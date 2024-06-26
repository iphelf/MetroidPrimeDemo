﻿using MetroidPrimeDemo.Scripts.Data;
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
            player.GetRotation(out _pitch, out _yaw);
            if (attributes.LockTarget is not null)
                return;

            Vector2 lookDelta = LookDelta();
            _yaw += lookDelta.x;
            _pitch += lookDelta.y;

            GeometryHelpers.Normalize(ref _pitch, ref _yaw);
            player.SetRotation(_pitch, _yaw);
        }
    }
}