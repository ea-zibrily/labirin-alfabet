using System;
using Alphabet.Enum;

namespace Alphabet.Gameplay.EventHandler
{
    public static class GameEventHandler
    {
        #region Game State Event

        //-- Dipanggil saat player memasuki finish point sebelum waktu habis
        public delegate void IsGameStart();
        public static event IsGameStart OnGameStart;
        
        //-- Dipanggil saat player memasuki finish point sebelum waktu habis
        public delegate void GameWin();
        public static event GameWin OnGameWin;
        
        //-- Dipanggil saat waktu/hp abis sebelum player memasuki finish point
        public delegate void GameOver(LoseType loseType);
        public static event GameOver OnGameOver;
        
        //-- Dipanggil saat semua objective sudah terpenuhi
        public delegate void ObjectiveClear();
        public static event ObjectiveClear OnObjectiveClear;
        
        //-- Dipanggil saat dalam satu level ada > 2 stage
        public delegate void ContinueStage();
        public static event ContinueStage OnContinueStage;
                
        #endregion
        
        #region Event Callbacks
        
        public static void GameStartEvent() => OnGameStart?.Invoke();
        public static void GameOverEvent(LoseType type) => OnGameOver?.Invoke(type);
        public static void GameWinEvent() => OnGameWin?.Invoke();
        public static void ObjectiveClearEvent() => OnObjectiveClear?.Invoke();
        public static void ContinueStageEvent() => OnContinueStage?.Invoke();

        #endregion
    }
}