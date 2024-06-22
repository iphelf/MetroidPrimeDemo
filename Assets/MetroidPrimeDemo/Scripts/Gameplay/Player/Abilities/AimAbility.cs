using MetroidPrimeDemo.Scripts.Data;
using UnityEngine.InputSystem;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player.Abilities
{
    public class AimAbility : Ability
    {
        private InputAction _input;

        public override void Initialize(InputConfig inputConfig, AbilityConfig abilityConfig)
        {
            _input = inputConfig.data.ActionsAsset.FindAction(inputConfig.data.action);
        }
    }
}