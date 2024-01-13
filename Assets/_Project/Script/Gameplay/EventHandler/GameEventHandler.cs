namespace CariHuruf.Gameplay.EventHandler
{
    public static class GameEventHandler
    {
        #region Initialize

        public delegate void GameStart();
        public static event GameStart OnGameStart;
        
        public delegate void GameOver();
        public static event GameOver OnGameOver;
        
        public delegate void GameWin();
        public static event GameWin OnGameWin;

        #endregion
        
        #region Event Callbacks
        
        public static void GameStartEvent() => OnGameStart?.Invoke();
        public static void GameOverEvent() => OnGameStart?.Invoke();
        public static void GameWinEvent() => OnGameStart?.Invoke();

        #endregion
    }
}