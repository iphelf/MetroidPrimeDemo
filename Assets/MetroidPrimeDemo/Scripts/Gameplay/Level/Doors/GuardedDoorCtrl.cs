using System;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Level.Doors
{
    [SelectionBase]
    public class GuardedDoorCtrl : MonoBehaviour
    {
        [SerializeField] private List<EnemyCharacterCtrl> guards;

        private void Start()
        {
            foreach (var guard in guards)
                guard.OnKill += OnEnemyKilled;
        }

        private void OnEnemyKilled(object sender, EventArgs _)
        {
            if (sender is not EnemyCharacterCtrl enemy) return;
            enemy.OnKill -= OnEnemyKilled;
            if (guards.Remove(enemy) && guards.Count == 0)
                Destroy(gameObject);
        }
    }
}