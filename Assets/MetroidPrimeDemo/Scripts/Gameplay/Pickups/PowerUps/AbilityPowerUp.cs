using MetroidPrimeDemo.Scripts.Data;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Pickups.PowerUps
{
    public class AbilityPowerUp : Pickup
    {
        [SerializeField] private InputConfig inputConfig;
        [SerializeField] private AbilityConfig abilityConfig;

        protected override void OnPickup(PlayerCharacterCtrl player)
        {
            player.abilities.GrantAbility(inputConfig, abilityConfig);
        }
    }
}