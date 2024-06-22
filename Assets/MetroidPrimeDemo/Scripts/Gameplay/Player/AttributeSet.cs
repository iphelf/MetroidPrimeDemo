using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player
{
    public class AttributeSet
    {
        #region Build

        // public float Health;

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