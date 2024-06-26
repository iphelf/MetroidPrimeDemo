using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Pickups.Floating
{
    public class EnergyPickup : FloatingPickup
    {
        [SerializeField] private float amount;

        protected override void OnPickup(PlayerCharacterCtrl player)
        {
            player.attributes.health = Mathf.Clamp(
                player.attributes.health + amount,
                0.0f, player.attributes.maxHealth
            );
        }
    }
}