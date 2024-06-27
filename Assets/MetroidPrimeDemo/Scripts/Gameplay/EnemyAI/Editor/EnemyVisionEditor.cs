using UnityEditor;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.EnemyAI.Editor
{
    [CustomEditor(typeof(EnemyVision))]
    public class EnemyVisionEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var vision = target as EnemyVision;
            if (!vision || !vision.Eye) return;
            Vector3 source = vision.Eye.position;
            Vector3 extend = vision.Eye.forward * vision.MaxDistance;
            Vector3 top = source + Quaternion.Euler(vision.ConeAngle * vision.Eye.right) * extend;
            Vector3 bottom = source + Quaternion.Euler(-vision.ConeAngle * vision.Eye.right) * extend;
            Vector3 left = source + Quaternion.Euler(vision.ConeAngle * vision.Eye.up) * extend;
            Vector3 right = source + Quaternion.Euler(-vision.ConeAngle * vision.Eye.up) * extend;
            Vector3 center = source + extend * Mathf.Cos(vision.ConeAngle * Mathf.Deg2Rad);
            Handles.color = Color.yellow;
            Handles.DrawLine(source, top);
            Handles.DrawLine(source, bottom);
            Handles.DrawLine(source, left);
            Handles.DrawLine(source, right);
            Handles.DrawWireDisc(
                center,
                vision.Eye.forward,
                vision.MaxDistance * Mathf.Sin(vision.ConeAngle * Mathf.Deg2Rad)
            );
            Handles.DrawWireArc(
                source,
                vision.Eye.up,
                right - source,
                vision.ConeAngle * 2.0f,
                vision.MaxDistance
            );
            Handles.DrawWireArc(
                source,
                vision.Eye.right,
                bottom - source,
                vision.ConeAngle * 2.0f,
                vision.MaxDistance
            );
        }
    }
}