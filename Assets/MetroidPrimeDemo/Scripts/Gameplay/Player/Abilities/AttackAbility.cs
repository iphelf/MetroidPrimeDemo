using MetroidPrimeDemo.Scripts.Data;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player.Abilities
{
    public abstract class SimpleAttackAbility : Ability
    {
        [SerializeField] private float damage;

        public override void Initialize(InputConfig inputConfig, AbilityConfig abilityConfig)
        {
            if (abilityConfig.data.TryReadConfig(nameof(damage), out var damageString))
                float.TryParse(damageString, out damage);
        }

        protected void OnDamage(GameObject other)
        {
            if (Aimable.IsValid(other, out var aimable))
                OnDamage(aimable);
        }

        protected void OnDamage(Aimable other)
        {
            if (other is EnemyCharacterCtrl enemy)
                enemy.DealDamage(damage);
            else
                Destroy(other.gameObject);
        }
    }
}