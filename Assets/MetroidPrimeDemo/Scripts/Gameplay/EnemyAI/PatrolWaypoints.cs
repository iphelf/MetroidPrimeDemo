using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.EnemyAI
{
    public class PatrolWaypoints : MonoBehaviour
    {
        public bool refreshOnEnable = true;
        [SerializeField] private List<Transform> waypoints;

        private void OnEnable()
        {
            if (refreshOnEnable) Refresh();
        }

        public int NearestWaypoint(Vector3 currentPosition, out Vector3 waypoint)
        {
            float minSqrDistance = float.PositiveInfinity;
            int minIndex = -1;
            for (int i = 0; i < waypoints.Count; ++i)
            {
                float sqrDistance = Vector3.SqrMagnitude(waypoints[i].position - currentPosition);
                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    minIndex = i;
                }
            }

            waypoint = minIndex == -1 ? transform.position : waypoints[minIndex].position;
            return minIndex;
        }

        public int NextWaypoint(int currentIndex, out Vector3 waypoint)
        {
            if (waypoints is null || waypoints.Count == 0)
            {
                waypoint = transform.position;
                return -1;
            }

            currentIndex = (currentIndex % waypoints.Count + waypoints.Count + 1) % waypoints.Count;
            waypoint = waypoints[currentIndex].position;
            return currentIndex;
        }

        public int NextWaypoint(out Vector3 waypoint) => NextWaypoint(-1, out waypoint);

        [Button]
        public void Refresh()
        {
            waypoints = Enumerable
                .Range(0, transform.childCount)
                .Select(transform.GetChild)
                .ToList();
        }

        private void OnDrawGizmos()
        {
            if (waypoints is null) return;
            Gizmos.color = Color.red;
            for (int i = 0; i < waypoints.Count; ++i)
            {
                Gizmos.DrawSphere(waypoints[i].position, 0.3f);
                if (i > 0)
                    Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
            }

            if (waypoints.Count > 1)
            {
                Gizmos.color = Color.Lerp(Color.white, Color.red, 0.7f);
                Gizmos.DrawLine(waypoints[^1].position, waypoints[0].position);
            }
        }
    }
}