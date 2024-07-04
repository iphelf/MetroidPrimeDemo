using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player.Abilities
{
    public abstract class SimpleAttackAbility : Ability
    {
        [SerializeField] private float damage;

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