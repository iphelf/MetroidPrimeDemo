namespace MetroidPrimeDemo.Scripts.Gameplay.Pickups.Upgrades
{
    public class EnergyTankPickup : Pickup
    {
        protected override void OnPickup(PlayerCharacterCtrl player)
        {
            player.attributes.maxHealth += 100.0f;
            player.attributes.health = player.attributes.maxHealth;
        }
    }
}