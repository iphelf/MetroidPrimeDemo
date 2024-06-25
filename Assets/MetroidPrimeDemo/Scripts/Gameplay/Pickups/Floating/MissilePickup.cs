using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Pickups.Floating
{
    public class MissilePickup : FloatingPickup
    {
        [SerializeField] private int count;
        private float _rotationSpeed = 100.0f;

        protected override void OnPickup(PlayerCharacterCtrl player)
        {
            player.Attributes.Missiles = Mathf.Clamp(
                player.Attributes.Missiles + count,
                0, player.Attributes.MaxMissiles
            );
        }

        private void Update()
        {
            transform.Rotate(0.0f, _rotationSpeed * Time.deltaTime, 0.0f, Space.Self);
        }
    }
}