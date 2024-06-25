using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    public abstract class Pickup : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<PlayerCharacterCtrl>();
            if (player == null) return;
            OnPickup(player);
            Destroy(gameObject);
        }

        protected abstract void OnPickup(PlayerCharacterCtrl player);
    }
}