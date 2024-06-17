using MetroidPrimeDemo.Scripts.General;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputs))]
    public class PlayerCharacterCtrl : MonoBehaviour
    {
        [SerializeField] private new Camera camera;

        private CharacterController _character;
        private PlayerInputs _inputs;

        [SerializeField] private float maxSpeedOnGround = 5.0f;
        [SerializeField] private float jumpSpeed = 9.0f;
        [SerializeField] private float maxSpeedInAir = 5.0f;
        [SerializeField] private float accelerationInAir = 5.0f;
        [SerializeField] private float gravity = 20.0f;
        [SerializeField] private float groundSnappingDistance = 0.25f;

        private float _cameraVerticalAngle;

        private bool _wasGrounded;
        private float _lastJumpTime = float.MinValue;
        private Vector3 _velocity;

        private void Start()
        {
            Debug.Assert(camera != null);
            _character = GetComponent<CharacterController>();
            _inputs = GetComponent<PlayerInputs>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            UpdateLook();
            UpdateGrounding();
            UpdateLocomotion();
        }

        private void UpdateGrounding()
        {
            if (Time.time - 0.5f < _lastJumpTime) return;

            float groundCheckDistance = _wasGrounded
                ? _character.skinWidth + groundSnappingDistance
                : _character.skinWidth + 0.01f;
            bool isGrounded = PhysicsHelpers.IsGrounded(
                _character, groundCheckDistance, -1,
                out var distanceFromGround);

            if (_wasGrounded && isGrounded)
            {
                // snapping to the ground while on the ground
                if (Mathf.Abs(distanceFromGround - _character.skinWidth) > 0.0f)
                    _character.Move(Vector3.down * (distanceFromGround - _character.skinWidth));
            }

            _wasGrounded = isGrounded;
        }

        private void UpdateLook()
        {
            Vector2 lookDelta = _inputs.LookDelta();

            transform.Rotate(0f, lookDelta.x, 0f, Space.Self);

            _cameraVerticalAngle += lookDelta.y;
            _cameraVerticalAngle = Mathf.Clamp(_cameraVerticalAngle, -89f, 89f);
            camera.transform.localEulerAngles = new Vector3(_cameraVerticalAngle, 0, 0);
        }

        private void UpdateLocomotion()
        {
            Vector2 movement = _inputs.Movement();
            Vector3 inputDir = transform.TransformVector(movement.x, 0.0f, movement.y);
            bool jumped = _inputs.Jumped();

            if (_wasGrounded)
            {
                _velocity = maxSpeedOnGround * inputDir;
                if (jumped)
                {
                    _velocity += jumpSpeed * Vector3.up;
                    _lastJumpTime = Time.time;
                    _wasGrounded = false;
                }
            }
            else
            {
                _velocity += Time.deltaTime * accelerationInAir * inputDir
                             + Time.deltaTime * gravity * Vector3.down;
                float verticalVelocity = _velocity.y;
                _velocity = Vector3.ProjectOnPlane(_velocity, Vector3.up);
                _velocity = Vector3.ClampMagnitude(_velocity, maxSpeedInAir);
                _velocity.y = verticalVelocity;
            }

            _character.Move(Time.deltaTime * _velocity);
        }
    }
}