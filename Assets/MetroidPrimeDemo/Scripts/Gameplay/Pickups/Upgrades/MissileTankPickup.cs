using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Pickups.Upgrades
{
    public class MissileTankPickup : Pickup
    {
        [SerializeField] private int count = 5;

        protected override void OnPickup(PlayerCharacterCtrl player)
        {
            player.attributes.maxMissiles += count;
            player.attributes.missiles += count;
        }
    }
}