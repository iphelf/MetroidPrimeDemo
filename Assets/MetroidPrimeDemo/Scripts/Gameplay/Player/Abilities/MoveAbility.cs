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
            base.Initialize(inputConfig, abilityConfig);
            _input = inputConfig.data.ActionsAsset.FindAction(inputConfig.data.action);
            if (inputConfig.data.TryReadConfig(nameof(smoothTime), out var smoothTimeString))
                float.TryParse(smoothTimeString, out smoothTime);
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

            if (attributes.wasGrounded)
                attributes.velocity = maxSpeedOnGround * inputDir;
            else
            {
                attributes.velocity += Time.deltaTime * accelerationInAir * inputDir;
                float verticalVelocity = attributes.velocity.y;
                attributes.velocity = Vector3.ProjectOnPlane(attributes.velocity, Vector3.up);
                attributes.velocity = Vector3.ClampMagnitude(attributes.velocity, maxSpeedInAir);
                attributes.velocity.y = verticalVelocity;
            }
        }
    }
}