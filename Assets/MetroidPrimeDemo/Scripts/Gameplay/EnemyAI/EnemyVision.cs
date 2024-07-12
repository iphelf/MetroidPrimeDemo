using System;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.EnemyAI
{
    public class EnemyVision : MonoBehaviour
    {
        [SerializeField] private Transform eye;
        [SerializeField] private float coneAngle = 48.0f;
        [SerializeField] private float maxDistance = 15.0f;
        [SerializeField] private LayerMask obstructionLayers = 0;

        public Transform Eye => eye;
        public float ConeAngle => coneAngle;
        public float MaxDistance => maxDistance;

        public Transform Target { get; private set; }

        public bool CanSee { get; private set; }
        public float LastTimeSeen { get; private set; }
        public Vector3 LastPositionSeen { get; private set; }

        private async void Start()
        {
            try
            {
                while (!destroyCancellationToken.IsCancellationRequested)
                {
                    UpdateCanSee();
                    await Awaitable.WaitForSecondsAsync(0.1f);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void UpdateCanSee()
        {
            CanSee = false;
            if (!eye || !Target) return;
            Vector3 direction = Target.position - eye.position;
            if (Vector3.Angle(eye.forward, direction) > coneAngle) return;
            float distance = direction.magnitude;
            if (distance > maxDistance) return;
            bool obstructed = Physics.Raycast(eye.position, direction, distance, obstructionLayers.value);
            if (obstructed) return;
            CanSee = true;
            LastTimeSeen = Time.time;
            LastPositionSeen = Target.position;
        }

        public void SetTarget(Transform target)
        {
            Target = target;
            LastTimeSeen = float.MinValue;
            LastPositionSeen = Vector3.positiveInfinity;
            CanSee = false;
        }
    }
}