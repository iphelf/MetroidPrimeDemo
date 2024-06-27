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

        public static Quaternion ClampEulerPitch(Quaternion rotation, float min, float max)
        {
            Vector3 euler = rotation.eulerAngles;
            euler.x %= 360.0f;
            if (euler.x > 180.0f) euler.x -= 360.0f;
            euler.x = Mathf.Clamp(euler.x, min, max);
            return Quaternion.Euler(euler);
        }
    }
}