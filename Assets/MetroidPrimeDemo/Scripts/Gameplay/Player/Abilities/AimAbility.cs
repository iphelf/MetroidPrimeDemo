using System;
using MetroidPrimeDemo.Scripts.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player.Abilities
{
    public class AimAbility : Ability
    {
        [SerializeField] private float radiusScale = 0.9f;
        [SerializeField] private LayerMask excludedAimLayerMask;

        private InputAction _input;
        private bool _wasAiming;
        private Vector3 _screenCenter;
        private float _sqrRadius;

        private void Awake()
        {
            excludedAimLayerMask = LayerMask.GetMask("Ignore Raycast");
        }

        private void Start()
        {
            _screenCenter = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
            _sqrRadius = Mathf.Min(_screenCenter.x, _screenCenter.y);
            _sqrRadius *= _sqrRadius;
        }

        public override void Initialize(InputConfig inputConfig, AbilityConfig abilityConfig)
        {
            _input = inputConfig.data.ActionsAsset.FindAction(inputConfig.data.action);
        }

        private void Update()
        {
            bool isAiming = _input.IsPressed();

            attributes.aimTarget = FindAimTarget();
            if (attributes.aimTarget is not null)
                attributes.aimTarget.OnDisabled += OnAimableDisabled;

            if (isAiming)
            {
                if (!_wasAiming)
                {
                    attributes.lockTarget = attributes.aimTarget;
                }
            }
            else
            {
                if (_wasAiming)
                    attributes.lockTarget = null;
            }

            _wasAiming = isAiming;
        }

        private void OnDisable()
        {
            _wasAiming = false;
            attributes.lockTarget = null;
            attributes.aimTarget = null;
            foreach (var aimable in Aimable.All)
                aimable.OnDisabled -= OnAimableDisabled;
        }

        private void OnAimableDisabled(object sender, EventArgs _)
        {
            var aimable = sender as Aimable;
            if (aimable is null) return;
            aimable.OnDisabled -= OnAimableDisabled;
            if (attributes.lockTarget == aimable)
                attributes.lockTarget = null;
        }

        private Aimable FindAimTarget()
        {
            float minSqrDistance = float.MaxValue;
            Aimable target = null;
            float maxSqrDistance = _sqrRadius * radiusScale * radiusScale;

            foreach (var aimable in Aimable.All)
            {
                {
                    Vector3 direction = aimable.PivotPosition - player.camera.transform.position;
                    float distance = direction.magnitude;
                    direction /= distance;
                    if (Vector3.Dot(player.camera.transform.forward, direction) < 0.0f)
                        continue;
                    bool isHit = Physics.Raycast(
                        player.camera.transform.position, direction,
                        out var hit, distance * 1.1f,
                        ~excludedAimLayerMask, QueryTriggerInteraction.Ignore);
                    if (!isHit)
                        continue;
                    // Debug.DrawLine(player.camera.transform.position, hit.point, Color.blue, 0.5f);
                    if (hit.transform.GetComponentInParent<Aimable>() != aimable)
                        continue;
                }

                Vector3 position = player.camera.WorldToScreenPoint(aimable.transform.position);
                float sqrDistance = Vector3.SqrMagnitude(position - _screenCenter);
                if (sqrDistance < maxSqrDistance && sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    target = aimable;
                }
            }

            return target;
        }
    }
}