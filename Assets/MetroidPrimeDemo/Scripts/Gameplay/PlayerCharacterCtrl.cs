using MetroidPrimeDemo.Scripts.Data;
using MetroidPrimeDemo.Scripts.Gameplay.Player;
using MetroidPrimeDemo.Scripts.General;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerCharacterCtrl : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private CannonCtrl cannon;

        private CharacterController _character;

        [SerializeField] private PlayerConfig config;
        [HideInInspector] public AttributeSet Attributes;

        [SerializeField] private float groundSnappingDistance = 0.25f;

        [SerializeField] private AbilitySystem abilities;

        private void Start()
        {
            _character = GetComponent<CharacterController>();

            Attributes = new AttributeSet
            {
                Gravity = config.gravity
            };

            abilities.Initialize(this);
            foreach (var entry in config.initialAbilities)
                abilities.GrantAbility(entry.input, entry.ability);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (Attributes.WasGrounded)
                Attributes.Velocity.y = 0.0f;
            else
                Attributes.Velocity.y -= Time.deltaTime * Attributes.Gravity;

            UpdateGrounding();

            _character.Move(Time.deltaTime * Attributes.Velocity);
        }

        private void UpdateGrounding()
        {
            if (Time.time - 0.5f < Attributes.LastJumpTime) return;

            float groundCheckDistance = Attributes.WasGrounded
                ? _character.skinWidth + groundSnappingDistance
                : _character.skinWidth + 0.01f;
            bool isGrounded = PhysicsHelpers.IsGrounded(
                _character, groundCheckDistance, -1,
                out var distanceFromGround);

            if (Attributes.WasGrounded && isGrounded)
            {
                // snapping to the ground while on the ground
                if (Mathf.Abs(distanceFromGround - _character.skinWidth) > 0.0f)
                    _character.Move(Vector3.down * (distanceFromGround - _character.skinWidth));
            }

            Attributes.WasGrounded = isGrounded;
        }

        public void SetRotation(float yaw, float pitch)
        {
            transform.localEulerAngles = new Vector3(0.0f, yaw, 0.0f);
            camera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }

        public void FireCannon()
        {
            cannon.Fire();
        }
    }
}