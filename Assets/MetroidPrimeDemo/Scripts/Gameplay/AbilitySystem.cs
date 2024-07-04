using System.Collections.Generic;
using MetroidPrimeDemo.Scripts.Data;
using MetroidPrimeDemo.Scripts.Gameplay.Player;
using MetroidPrimeDemo.Scripts.Gameplay.Player.Abilities;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    public class AbilitySystem : MonoBehaviour
    {
        private PlayerCharacterCtrl _player;
        private readonly SortedDictionary<AbilityType, AbilityConfig> _abilities = new();

        public void Initialize(PlayerCharacterCtrl player)
        {
            _player = player;
        }

        public void GrantAbility(InputConfig inputConfig, AbilityConfig abilityConfig)
        {
            if (!_abilities.TryAdd(abilityConfig.data.type, abilityConfig)) return;
            Ability ability = InstantiateAbility(abilityConfig.data.type);
            ability.Initialize(inputConfig, abilityConfig);
        }

        public bool ContainsAbility(AbilityType type) => _abilities.ContainsKey(type);

        public IEnumerable<AbilityConfig> Abilities => _abilities.Values;

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
                ability.attributes = _player.attributes;
                ability.player = _player;
            }

            return ability;
        }
    }
}