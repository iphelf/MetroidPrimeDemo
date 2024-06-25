using MetroidPrimeDemo.Scripts.Data;
using UnityEngine.InputSystem;

namespace MetroidPrimeDemo.Scripts.Gameplay.Player.Abilities
{
    public class FireBeamAbility : Ability
    {
        private InputAction _input;
        private bool _wasFiringBeam;

        public override void Initialize(InputConfig inputConfig, AbilityConfig abilityConfig)
        {
            _input = inputConfig.data.ActionsAsset.FindAction(inputConfig.data.action);
        }

        private bool FiringBeam() => _input.IsPressed();

        private void Update()
        {
            bool isFiringBeam = FiringBeam();
            if (!_wasFiringBeam && isFiringBeam)
                player.cannon.Fire();
            _wasFiringBeam = isFiringBeam;
        }
    }
}