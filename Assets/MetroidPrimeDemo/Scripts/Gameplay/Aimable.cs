using System;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    [SelectionBase]
    public class Aimable : MonoBehaviour
    {
        private static readonly HashSet<Aimable> EnabledAimableSet = new();
        public static IEnumerable<Aimable> All => EnabledAimableSet;

        public static bool IsValid(GameObject go, out Aimable aimable)
        {
            aimable = go.GetComponent<Aimable>();
            return aimable != null;
        }

        public static bool IsValid(Collider collider, out Aimable aimable)
            => IsValid(collider.attachedRigidbody.gameObject, out aimable);

        [SerializeField] private Transform aimPivot;
        public Vector3 PivotPosition => (!aimPivot ? transform : aimPivot).position;
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