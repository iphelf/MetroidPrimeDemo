using UnityEngine;

namespace MetroidPrimeDemo.Scripts.General
{
    public static class GeometryHelpers
    {
        public static void LookAt(Vector3 source, Vector3 target, out float pitch, out float yaw)
        {
            Vector3 direction = (target - source).normalized;
            pitch = Mathf.Asin(-direction.y) * Mathf.Rad2Deg;
            yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
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