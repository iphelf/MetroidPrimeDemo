using UnityEditor;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.EnemyAI.Editor
{
    [CustomEditor(typeof(EnemyHearing))]
    public class EnemyHearingEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var hearing = target as EnemyHearing;
            if (!hearing?.Ear) return;
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(hearing.Ear.position, hearing.Ear.up, hearing.MaxDistance);
        }
    }
}