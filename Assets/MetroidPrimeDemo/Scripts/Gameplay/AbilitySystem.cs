using MetroidPrimeDemo.Scripts.Data;
using MetroidPrimeDemo.Scripts.Gameplay.Player;
using MetroidPrimeDemo.Scripts.Gameplay.Player.Abilities;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    public class AbilitySystem : MonoBehaviour
    {
        private PlayerCharacterCtrl _player;

        public void Initialize(PlayerCharacterCtrl player)
        {
            _player = player;
        }

        public void GrantAbility(InputConfig inputConfig, AbilityConfig abilityConfig)
        {
            Ability ability = InstantiateAbility(abilityConfig.data.type);
            ability.Initialize(inputConfig, abilityConfig);
        }

        private Ability InstantiateAbility(AbilityType type)
        {
            Ability ability = type switch
            {
                AbilityType.Jump => gameObject.AddComponent<JumpAbility>(),
                AbilityType.Move => gameObject.AddComponent<MoveAbility>(),
                AbilityType.Look => gameObject.AddComponent<LookAbility>(),
                AbilityType.FireBeam => gameObject.AddComponent<FireBeamAbility>(),
                AbilityType.Aim => gameObject.AddComponent<AimAbility>(),
                AbilityType.FireMissile => gameObject.AddComponent<FireMissileAbility>(),
                AbilityType.ChargeBeam => gameObject.AddComponent<ChargeBeamAbility>(),
                _ => null,
            };
            if (ability != null)
            {
                ability.Attributes = _player.Attributes;
                ability.player = _player;
            }

            return ability;
        }
    }
}