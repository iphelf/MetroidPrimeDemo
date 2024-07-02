using System;
using MetroidPrimeDemo.Scripts.Data;
using MetroidPrimeDemo.Scripts.Gameplay.Player;
using MetroidPrimeDemo.Scripts.Gameplay.Weapons;
using MetroidPrimeDemo.Scripts.General;
using MetroidPrimeDemo.Scripts.Modules;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    [SelectionBase]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerCharacterCtrl : MonoBehaviour
    {
        public new Camera camera;
        public CannonCtrl cannon;

        private CharacterController _character;

        [SerializeField] private PlayerConfig config;
        public AttributeSet attributes;

        [SerializeField] private float groundSnappingDistance = 0.25f;

        public AbilitySystem abilities;

        public Vector3 Center => transform.position + Vector3.up;

        private float _pitch;
        private float _yaw;

        private void Start()
        {
            _character = GetComponent<CharacterController>();

            attributes = new AttributeSet
            {
                gravity = config.gravity
            };

            abilities.Initialize(this);
            foreach (var entry in config.initialAbilities)
                if (entry.enabled)
                    abilities.GrantAbility(entry.input, entry.ability);

            _yaw = transform.localEulerAngles.y;
            _pitch = camera.transform.localEulerAngles.x;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            if (attributes.wasGrounded)
                attributes.velocity.y = 0.0f;
            else
                attributes.velocity.y -= Time.deltaTime * attributes.gravity;

            UpdateGrounding();

            _character.Move(Time.deltaTime * attributes.velocity);
        }

        private void UpdateGrounding()
        {
            if (Time.time - 0.5f < attributes.lastJumpTime) return;

            float groundCheckDistance = attributes.wasGrounded
                ? _character.skinWidth + groundSnappingDistance
                : _character.skinWidth + 0.01f;
            bool isGrounded = PhysicsHelpers.IsGrounded(
                _character, groundCheckDistance, -1,
                out var distanceFromGround);

            if (attributes.wasGrounded && isGrounded)
            {
                // snapping to the ground while on the ground
                if (Mathf.Abs(distanceFromGround - _character.skinWidth) > 0.0f)
                    _character.Move(Vector3.down * (distanceFromGround - _character.skinWidth));
            }

            attributes.wasGrounded = isGrounded;
        }

        public void GetRotation(out float pitch, out float yaw)
        {
            pitch = _pitch;
            yaw = _yaw;
        }

        public void SetRotation(float pitch, float yaw)
        {
            _yaw = yaw;
            _pitch = pitch;
            transform.localEulerAngles = new Vector3(0.0f, _yaw, 0.0f);
            camera.transform.localEulerAngles = new Vector3(_pitch, 0, 0);
        }

        public void Hurt(float damage)
        {
            attributes.health = Mathf.Clamp(attributes.health - damage, 0.0f, attributes.maxHealth);
            if (attributes.health == 0.0f)
                GameFlow.FinishGame(this);
        }
    }
}