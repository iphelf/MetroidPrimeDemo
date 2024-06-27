using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    public class EnemyCharacterCtrl : Aimable
    {
        public float maxHealth;
        private float _health;
        protected PlayerCharacterCtrl Player;
        private bool _destroying;

        protected virtual void Awake()
        {
            var go = GameObject.FindWithTag("Player");
            Player = go.GetComponent<PlayerCharacterCtrl>();
        }

        public void DealDamage(float damage)
        {
            if (_destroying) return;

            _health = Mathf.Clamp(_health - damage, 0.0f, maxHealth);
            if (_health == 0.0f)
            {
                _destroying = true;
                OnDamaged();
            }
        }

        protected virtual void OnDamaged()
        {
            Destroy(gameObject);
        }
    }
}