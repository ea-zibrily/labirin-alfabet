using System;
using UnityEngine;
using CariHuruf.Enum;
using CariHuruf.Managers;
using CariHuruf.Entities.Player;
using CariHuruf.Gameplay.Controller;
using CariHuruf.Gameplay.EventHandler;
using CariHuruf.DesignPattern.Singleton;

namespace CariHuruf.Entities.Item
{
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Enums

        private enum LevelState
        {
            Current,
            Previous,
            None
        }

        #endregion
        
        #region Variable
        
        [Header("Level Settings")] 
        public Level CurrentLevel;
        public Stage CurrentStage;
        public Stage NextStage;
        
        [Space]
        public Level PreviousLevel;
        public Stage PreviousStage;
        
        private string _timeSaveKey;

        [Header("UI")] 
        [SerializeField] private GameObject gameWinPanel;
        [SerializeField] private GameObject gameOverPanel;
        
        [Header("Reference")] 
        private PlayerController _playerController;
        private TimeController _timeController;

        private StarRatingManager _starRatingManager;
        
        #endregion

        #region MonoBehaviour Callbacks
        
        protected override void Awake()
        {
            base.Awake();
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
            _starRatingManager = GameObject.Find("StarRatingManager").GetComponent<StarRatingManager>();
        }

        private void OnEnable()
        {
            GameEventHandler.OnGameStart += GameStart;
            GameEventHandler.OnGameWin += GameWin;
            GameEventHandler.OnGameOver += GameOver;
            GameEventHandler.OnContinueStage += ContinueStage;
        }
        
        private void OnDisable()
        {
            GameEventHandler.OnGameStart -= GameStart;
            GameEventHandler.OnGameWin -= GameWin;
            GameEventHandler.OnGameOver -= GameOver;
            GameEventHandler.OnContinueStage += ContinueStage;
        }
        
        #endregion
        
        #region Game State Callbacks
        
        //-- Core Functionality
        private void GameStart()
        {
            
        }
        
        private void GameOver()
        {
            
        }
        
        private void GameWin()
        {
            _playerController.StopMovement();
            _starRatingManager.RateStar();
            // SaveCurrentTime();
        }
        
        private void ContinueStage()
        {
            _playerController.StopMovement();
            SaveCurrentTime();
            SceneTransitionManager.Instance.LoadNextScene();
        }
        
        #endregion
        
        #region Load and Save Callbacks
        
        //-- Initialization
        private string SetTimePrefsKey(LevelState state)
        {
            var currentKey = state switch
            {
                LevelState.Current => CurrentLevel + "_" + CurrentStage,
                LevelState.Previous => PreviousLevel + "_" + PreviousStage,
                LevelState.None => "None!",
                _ => ""
            };
            
            return currentKey;
        }
        
        //-- Helpers/Utilities
        public float GetLatestTime()
        {
            _timeSaveKey = SetTimePrefsKey(LevelState.Previous);
            var lastTime = PlayerPrefs.GetFloat(_timeSaveKey);
            return lastTime;
        }
        
        private void SaveCurrentTime()
        {
            _timeSaveKey = SetTimePrefsKey(LevelState.Current);
            if (PlayerPrefs.HasKey(_timeSaveKey))
            {
                PlayerPrefs.SetFloat(_timeSaveKey, _timeController.CurrentTime);
            }
        }
        
        #endregion

        #region Utilites

        public bool CanNextStage() => NextStage != Stage.None;

        #endregion
    }
}