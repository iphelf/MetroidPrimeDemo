using System;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.EnemyAI
{
    public class EnemyHearing : MonoBehaviour
    {
        [field: SerializeField] public Transform Ear { get; private set; }
        [field: SerializeField] public float MaxDistance { get; private set; } = 5.0f;

        public Transform Target { get; private set; }

        public bool CanHear { get; private set; }
        public float LastTimeHeard { get; private set; }
        public Vector3 LastPositionHeard { get; private set; }

        private async void Start()
        {
            try
            {
                while (!destroyCancellationToken.IsCancellationRequested)
                {
                    UpdateCanHear();
                    await Awaitable.WaitForSecondsAsync(0.1f);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void UpdateCanHear()
        {
            CanHear = false;
            if (!Ear || !Target) return;
            float distance = Vector3.Distance(Ear.position, Target.position);
            if (distance > MaxDistance) return;
            CanHear = true;
            LastTimeHeard = Time.time;
            LastPositionHeard = Target.position;
        }

        public void SetTarget(Transform target)
        {
            Target = target;
            LastTimeHeard = float.MinValue;
            LastPositionHeard = Vector3.positiveInfinity;
            CanHear = false;
        }
    }
}