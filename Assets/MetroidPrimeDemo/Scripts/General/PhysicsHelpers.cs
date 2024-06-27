using UnityEngine;

namespace MetroidPrimeDemo.Scripts.General
{
    public static class PhysicsHelpers
    {
        public static bool IsGrounded(
            CharacterController character, float maxDistance, int layerMask,
            out float distanceFromGround)
        {
            distanceFromGround = float.NaN;

            var transform = character.transform;
            var position = transform.position + character.center;
            var up = transform.up;
            var capsulePoint1 = position - (character.height / 2 - character.radius) * up;
            var capsulePoint2 = position + (character.height / 2 - character.radius) * up;
            if (!Physics.CapsuleCast(
                    capsulePoint1, capsulePoint2, character.radius,
                    Vector3.down,
                    out RaycastHit hit,
                    maxDistance, layerMask, QueryTriggerInteraction.Ignore))
                return false;
            if (Vector3.Dot(hit.normal, up) <= 0.0f) return false;
            if (Vector3.Angle(hit.normal, up) > character.slopeLimit) return false;
            distanceFromGround = hit.distance;
            return true;
        }
    }
}