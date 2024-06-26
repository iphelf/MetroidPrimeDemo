using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    public class EnemyCharacterCtrl : Aimable
    {
        public float maxHealth;
        private float _health;

        public void DealDamage(float damage)
        {
            _health = Mathf.Clamp(_health - damage, 0.0f, maxHealth);
            if (_health == 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}