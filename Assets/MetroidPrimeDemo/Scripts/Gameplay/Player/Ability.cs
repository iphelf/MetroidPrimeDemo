using MetroidPrimeDemo.Scripts.Data;
using MetroidPrimeDemo.Scripts.General;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player
{
    public abstract class Ability : MonoBehaviour
    {
        [HideInInspector] public AttributeSet attributes;

        [HideInInspector] public PlayerCharacterCtrl player;

        public virtual void Initialize(InputConfig inputConfig, AbilityConfig abilityConfig)
        {
            ConfigHelpers.ApplySerializableConfigs(abilityConfig.data.config, this);
            ConfigHelpers.ApplySerializableConfigs(abilityConfig.data.dependencies, this);
        }
    }
}