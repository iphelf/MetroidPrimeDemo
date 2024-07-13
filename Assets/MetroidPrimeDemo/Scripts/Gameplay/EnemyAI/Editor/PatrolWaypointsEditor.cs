using UnityEditor;

namespace MetroidPrimeDemo.Scripts.Gameplay.EnemyAI.Editor
{
    [CustomEditor(typeof(PatrolWaypoints))]
    public class PatrolWaypointsEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var waypoints = target as PatrolWaypoints;
            if (!waypoints || !waypoints.refreshOnEnable) return;
            waypoints.Refresh();
        }
    }
}