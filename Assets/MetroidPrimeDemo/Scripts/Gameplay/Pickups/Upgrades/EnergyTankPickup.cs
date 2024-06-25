namespace MetroidPrimeDemo.Scripts.Gameplay.Pickups.Upgrades
{
    public class EnergyTankPickup : Pickup
    {
        protected override void OnPickup(PlayerCharacterCtrl player)
        {
            player.Attributes.MaxHealth += 100.0f;
            player.Attributes.Health = player.Attributes.MaxHealth;
        }
    }
}