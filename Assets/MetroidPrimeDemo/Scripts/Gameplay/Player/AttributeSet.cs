using System;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player
{
    [Serializable]
    public class AttributeSet
    {
        #region Build

        [Header("Build")] public float MaxHealth;
        public int MaxMissiles;

        #endregion

        #region Stats

        [Header("Stats")] public float Health;
        public int Missiles;

        #endregion

        #region Environmental

        [Header("Environmental")] public float Gravity;

        #endregion

        #region Temporary

        [Header("Temporary")] public Vector3 Velocity;
        public bool WasGrounded;
        public float LastJumpTime = float.MinValue;
        public Aimable AimTarget;
        public Aimable LockTarget;

        #endregion
    }
}