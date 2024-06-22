using MetroidPrimeDemo.Scripts.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player.Abilities
{
    public class MoveAbility : Ability
    {
        [SerializeField] private float smoothTime = 0.1f;

        [SerializeField] private float maxSpeedOnGround = 5.0f;
        [SerializeField] private float maxSpeedInAir = 5.0f;
        [SerializeField] private float accelerationInAir = 5.0f;

        private InputAction _input;
        private Vector2 _currentMove;
        private Vector2 _currentMoveDampVelocity;

        public override void Initialize(InputConfig inputConfig, AbilityConfig abilityConfig)
        {
            _input = inputConfig.data.ActionsAsset.FindAction(inputConfig.data.action);
            if (inputConfig.data.TryReadConfig(nameof(smoothTime), out var smoothTimeString))
                float.TryParse(smoothTimeString, out smoothTime);
            if (abilityConfig.data.TryReadConfig(nameof(maxSpeedOnGround), out var maxSpeedOnGroundString))
                float.TryParse(maxSpeedOnGroundString, out maxSpeedOnGround);
            if (abilityConfig.data.TryReadConfig(nameof(maxSpeedInAir), out var maxSpeedInAirString))
                float.TryParse(maxSpeedInAirString, out maxSpeedInAir);
            if (abilityConfig.data.TryReadConfig(nameof(accelerationInAir), out var accelerationInAirString))
                float.TryParse(accelerationInAirString, out accelerationInAir);
        }

        private Vector2 Movement()
        {
            var move = _input.ReadValue<Vector2>();
            _currentMove = Vector2.SmoothDamp(_currentMove, move, ref _currentMoveDampVelocity, smoothTime);
            return _currentMove;
        }

        public void Update()
        {
            Vector2 movement = Movement();
            Vector3 inputDir = transform.TransformVector(movement.x, 0.0f, movement.y);

            if (Attributes.WasGrounded)
                Attributes.Velocity = maxSpeedOnGround * inputDir;
            else
            {
                Attributes.Velocity += Time.deltaTime * accelerationInAir * inputDir;
                float verticalVelocity = Attributes.Velocity.y;
                Attributes.Velocity = Vector3.ProjectOnPlane(Attributes.Velocity, Vector3.up);
                Attributes.Velocity = Vector3.ClampMagnitude(Attributes.Velocity, maxSpeedInAir);
                Attributes.Velocity.y = verticalVelocity;
            }
        }
    }
}