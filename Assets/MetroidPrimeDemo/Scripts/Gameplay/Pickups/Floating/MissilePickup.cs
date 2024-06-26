using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Pickups.Floating
{
    public class MissilePickup : FloatingPickup
    {
        [SerializeField] private int count;
        private float _rotationSpeed = 100.0f;

        protected override void OnPickup(PlayerCharacterCtrl player)
        {
            player.attributes.missiles = Mathf.Clamp(
                player.attributes.missiles + count,
                0, player.attributes.maxMissiles
            );
        }

        private void Update()
        {
            transform.Rotate(0.0f, _rotationSpeed * Time.deltaTime, 0.0f, Space.Self);
        }
    }
}