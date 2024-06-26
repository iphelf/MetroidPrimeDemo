using UnityEngine;

namespace MetroidPrimeDemo.Scripts.General
{
    public static class GeometryHelpers
    {
        public static void Look(Vector3 forward, out float pitch, out float yaw)
        {
            var euler = Quaternion.LookRotation(forward, Vector3.up).eulerAngles;
            pitch = euler.x;
            yaw = euler.y;
        }

        public static void Normalize(ref float pitch, ref float yaw)
        {
            yaw %= 360.0f;

            pitch %= 360.0f;
            if (pitch > 180.0f) pitch -= 360.0f;
            pitch = Mathf.Clamp(pitch, -89f, 89f);
        }
    }
}