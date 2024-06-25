using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Pickups.Floating
{
    public class EnergyPickup : FloatingPickup
    {
        [SerializeField] private float amount;

        protected override void OnPickup(PlayerCharacterCtrl player)
        {
            player.Attributes.Health = Mathf.Clamp(
                player.Attributes.Health + amount,
                0.0f, player.Attributes.MaxHealth
            );
        }
    }
}