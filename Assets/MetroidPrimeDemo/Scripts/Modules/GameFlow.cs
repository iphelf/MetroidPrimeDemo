namespace MetroidPrimeDemo.Scripts.Modules
{
    public static class GameFlow
    {
        public static void NewGame()
        {
        }

        public record GameOutcome(bool Success, float Completion, float Time);

        public static GameOutcome Outcome => new GameOutcome(
            Success: false,
            Completion: 9.0f,
            Time: 1.0f
        );

        public static void RetryLevel()
        {
            NewGame();
        }

        public static void ReturnToMainMenu()
        {
        }
    }
}