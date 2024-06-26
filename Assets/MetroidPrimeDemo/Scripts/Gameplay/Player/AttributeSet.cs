using System;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player
{
    [Serializable]
    public class AttributeSet
    {
        #region Build

        [Header("Build")] public float maxHealth;
        public int maxMissiles;

        #endregion

        #region Stats

        [Header("Stats")] public float health;
        public int missiles;

        #endregion

        #region Environmental

        [Header("Environmental")] public float gravity;

        #endregion

        #region Temporary

        [Header("Temporary")] public Vector3 velocity;
        public bool wasGrounded;
        public float lastJumpTime = float.MinValue;
        public Aimable aimTarget;
        public Aimable lockTarget;

        #endregion
    }
}