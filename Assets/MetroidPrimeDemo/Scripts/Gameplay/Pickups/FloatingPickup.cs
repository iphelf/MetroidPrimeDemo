using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Pickups
{
    public abstract class FloatingPickup : Pickup
    {
        private float _moveSpeed = 2.0f;

        private void Start()
        {
            tag = "FloatingPickup";
        }

        public void MoveTowards(Vector3 target)
        {
            Vector3 vector = target - transform.position;
            float maxMovement = _moveSpeed * Time.deltaTime;
            if (maxMovement * maxMovement > vector.sqrMagnitude)
                transform.position = target;
            else
                transform.position += vector.normalized * maxMovement;
        }
    }
}