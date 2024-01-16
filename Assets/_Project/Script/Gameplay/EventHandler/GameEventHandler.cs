namespace LabirinKata.Gameplay.EventHandler
{
    public static class GameEventHandler
    {
        #region Game State Event
        
        //-- Dipanggil saat game mulai
        public delegate void GameStart();
        public static event GameStart OnGameStart;
        
        //-- Dipanggil saat waktu habis sebelum player memasuki finish point
        public delegate void GameOver();
        public static event GameOver OnGameOver;
        
        //-- Dipanggil saat player memasuki finish point sebelum waktu habis
        public delegate void GameWin();
        public static event GameWin OnGameWin;
        
        //-- Dipanggil saat semua objective sudah terpenuhi
        public delegate void ObjectiveClear();
        public static event ObjectiveClear OnObjectiveClear;
        
        //-- Dipanggil saat dalam satu level ada > 2 stage
        public delegate void ContinueStage();
        public static event ContinueStage OnContinueStage;
        
        #endregion
        
        #region Event Callbacks
        
        public static void GameStartEvent() => OnGameStart?.Invoke();
        public static void GameOverEvent() => OnGameOver?.Invoke();
        public static void GameWinEvent() => OnGameWin?.Invoke();
        public static void ObjectiveClearEvent() => OnObjectiveClear?.Invoke();
        public static void ContinueStageEvent() => OnContinueStage?.Invoke();
        
        #endregion
    }
}