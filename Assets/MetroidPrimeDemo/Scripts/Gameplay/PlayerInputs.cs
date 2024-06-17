using UnityEngine;
using UnityEngine.InputSystem;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    public class PlayerInputs : MonoBehaviour
    {
        [SerializeField] private InputActionAsset actions;

        [SerializeField] private float moveSmoothTime = 0.1f;
        private Vector2 _currentMove;
        private Vector2 _currentMoveDampVelocity;

        private InputAction _moveAction;

        [SerializeField] private float gamepadRotationSpeed = 150.0f;
        [SerializeField] private float mouseSensitivity = 0.08f;

        private InputAction _lookByStickAction;
        private InputAction _lookByPointerAction;

        private InputAction _aimAction;
        private InputAction _fireBeamAction;
        private InputAction _fireMissileAction;
        private InputAction _jumpAction;

        private bool _disabled = false;

        private void Start()
        {
            if (actions == null)
                actions = InputSystem.actions;

            _moveAction = actions.FindAction("Player/Move", true);
            _lookByStickAction = actions.FindAction("Player/LookByStick", true);
            _lookByPointerAction = actions.FindAction("Player/LookByPointer", true);
            _aimAction = actions.FindAction("Player/Aim", true);
            _fireBeamAction = actions.FindAction("Player/FireBeam", true);
            _fireMissileAction = actions.FindAction("Player/FireMissile", true);
            _jumpAction = actions.FindAction("Player/Jump", true);
        }

        public Vector2 Movement()
        {
            if (_disabled) return Vector2.zero;

            var move = _moveAction.ReadValue<Vector2>();
            _currentMove = Vector2.SmoothDamp(_currentMove, move, ref _currentMoveDampVelocity, moveSmoothTime);
            return _currentMove;
        }

        public Vector2 LookDelta()
        {
            if (_disabled) return Vector2.zero;

            var look = _lookByStickAction.ReadValue<Vector2>();
            // Check if this look input is coming from the mouse
            if (look.Equals(Vector2.zero))
                look = _lookByPointerAction.ReadValue<Vector2>() * mouseSensitivity;
            // or from the gamepad
            else
                look *= Time.deltaTime * gamepadRotationSpeed;

            look.y = -look.y;

            return look;
        }

        public bool FiringBeam() => !_disabled && _fireBeamAction.IsPressed();

        public bool FiredMissile() => !_disabled && _fireMissileAction.WasPressedThisFrame();

        public bool Jumped() => !_disabled && _jumpAction.WasPressedThisFrame();
    }
}