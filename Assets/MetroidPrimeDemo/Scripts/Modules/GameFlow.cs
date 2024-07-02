using MetroidPrimeDemo.Scripts.Data;
using MetroidPrimeDemo.Scripts.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MetroidPrimeDemo.Scripts.Modules
{
    public static class GameFlow
    {
        private static float _lastGameBeginTime = float.MinValue;

        public static void NewGame()
        {
            _lastGameBeginTime = Time.time;
            SceneManager.LoadScene("Level");
        }

        public record GameOutcome(bool Success, float Completion, float Time);

        public static GameOutcome Outcome { get; private set; }

        public static void FinishGame(PlayerCharacterCtrl player)
        {
            bool alive = player.attributes.health > 0;
            int energyTanks = Mathf.FloorToInt(player.attributes.maxHealth / 100.0f);
            int missileTanks = player.attributes.maxMissiles / 5;
            bool hasMissileAbility = player.abilities.ContainsAbility(AbilityType.FireMissile);
            bool hasChargeBeamAbility = player.abilities.ContainsAbility(AbilityType.ChargeBeam);
            int obtained = energyTanks + missileTanks + (hasMissileAbility ? 1 : 0) + (hasChargeBeamAbility ? 1 : 0);
            int total = 1 + 1 + 1 + 1;
            float completion = obtained * 1.0f / total;
            Outcome = new GameOutcome(alive, completion, Time.time - _lastGameBeginTime);


            SceneManager.LoadScene("GameOver");
        }

        public static void RetryLevel()
        {
            NewGame();
        }

        public static void ReturnToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}