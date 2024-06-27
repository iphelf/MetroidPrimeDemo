using System;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    public class Aimable : MonoBehaviour
    {
        private static readonly HashSet<Aimable> EnabledAimableSet = new();
        private static int _layer = -1;
        private static int _layerMask = -1;

        public static int Layer
            => _layer == -1 ? _layer = UnityEngine.LayerMask.NameToLayer("Aimable") : _layer;

        public static int LayerMask
            => _layerMask == -1 ? _layerMask = UnityEngine.LayerMask.GetMask("Aimable") : _layerMask;

        public static IEnumerable<Aimable> All => EnabledAimableSet;

        public static bool IsValid(GameObject go, out Aimable aimable)
        {
            aimable = go.GetComponent<Aimable>();
            return aimable != null;
        }

        public string targetName = "(Target)";
        public EventHandler OnDisabled;

        protected virtual void OnEnable()
        {
            EnabledAimableSet.Add(this);
        }

        protected virtual void OnDisable()
        {
            if (EnabledAimableSet.Remove(this))
                OnDisabled?.Invoke(this, null);
        }
    }
}