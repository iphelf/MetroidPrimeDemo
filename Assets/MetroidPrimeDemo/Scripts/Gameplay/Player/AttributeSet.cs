using System;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player
{
    [Serializable]
    public class AttributeSet
    {
        #region Build

        public float MaxHealth;
        public int MaxMissiles;

        #endregion

        #region Stats

        public float Health;
        public int Missiles;

        #endregion

        #region Environmental

        public float Gravity;

        #endregion

        #region Temporary

        public Vector3 Velocity;
        public bool WasGrounded;
        public float LastJumpTime = float.MinValue;

        #endregion
    }
}