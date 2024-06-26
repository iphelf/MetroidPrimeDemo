using System.Collections.Generic;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    public class Aimable : MonoBehaviour
    {
        public static readonly HashSet<Aimable> EnabledAimableSet = new();

        private void OnEnable()
        {
            EnabledAimableSet.Add(this);
        }

        private void OnDisable()
        {
            EnabledAimableSet.Remove(this);
        }
    }
}