using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.EnemyAI
{
    public class EnemyVision : MonoBehaviour
    {
        [SerializeField] private Transform eye;
        [SerializeField] private float coneAngle = 48.0f;
        [SerializeField] private float maxDistance = 15.0f;
        [SerializeField] private LayerMask obstructionLayers;

        public Transform Eye => eye;
        public float ConeAngle => coneAngle;
        public float MaxDistance => maxDistance;

        public bool CanSee(Vector3 target)
        {
            if (!eye) return false;
            Vector3 direction = target - eye.position;
            if (Vector3.Angle(eye.forward, direction) > coneAngle) return false;
            float distance = direction.magnitude;
            if (distance > maxDistance) return false;
            bool obstructed = Physics.Raycast(eye.position, direction, distance, obstructionLayers.value);
            return !obstructed;
        }
    }
}