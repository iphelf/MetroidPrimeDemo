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

        [SerializeField] private float maxSpeed = 8.0f;

        private float _cameraVerticalAngle;

        private void Start()
        {
            Debug.Assert(camera != null);
            _character = GetComponent<CharacterController>();
            _inputs = GetComponent<PlayerInputs>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Application.targetFrameRate = 120;
        }

        private void Update()
        {
            var look = _inputs.Look();

            // horizontal character rotation
            {
                // rotate the transform with the input speed around its local Y axis
                transform.Rotate(0f, look.x, 0f, Space.Self);
            }

            // vertical camera rotation
            {
                // add vertical inputs to the camera's vertical angle
                _cameraVerticalAngle += look.y;

                // limit the camera's vertical angle to min/max
                _cameraVerticalAngle = Mathf.Clamp(_cameraVerticalAngle, -89f, 89f);

                // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
                camera.transform.localEulerAngles = new Vector3(_cameraVerticalAngle, 0, 0);
            }

            // character movement
            {
                var movement2D = maxSpeed * Time.deltaTime * _inputs.Move();
                Vector3 movement = transform.TransformVector(movement2D.x, 0.0f, movement2D.y);
                _character.Move(movement);
            }
        }
    }
}