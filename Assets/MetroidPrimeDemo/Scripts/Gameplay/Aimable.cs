using System;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    public class Aimable : MonoBehaviour
    {
        public static readonly HashSet<Aimable> EnabledAimableSet = new();
        public EventHandler OnDisabled;

        private void OnEnable()
        {
            EnabledAimableSet.Add(this);
        }

        private void OnDisable()
        {
            EnabledAimableSet.Remove(this);
            OnDisabled?.Invoke(this, null);
        }
    }
}