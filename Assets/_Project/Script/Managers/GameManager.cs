﻿using System;
using System.Collections;
using UnityEngine;
using LabirinKata.Stage;
using LabirinKata.Entities.Item;
using LabirinKata.Entities.Player;
using LabirinKata.Gameplay.Controller;
using LabirinKata.Gameplay.EventHandler;
using LabirinKata.DesignPattern.Singleton;

namespace LabirinKata.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Constant Variable
        
        //-- Load stage time delay
        private const float FADE_OUT_DELAY = 2.5f;
        private const float LOAD_STAGE_DELAY = 1.2f;
        private const float FADE_IN_DELAY = 1.5f;
        
        #endregion
        
        #region Variable

        [Header("UI")] 
        [SerializeField] private GameObject gameWinPanelUI;
        [SerializeField] private GameObject gameOverPanelUI;
        [SerializeField] private GameObject notificationStagePanelUI;
        
        [Header("Settings")]
        private string _saveKey;
        public bool IsGameStart { get; private set; }
        
        [Header("Reference")] 
        private PlayerController _playerController;
        private TimeController _timeController;
        
        private LetterManager _letterManager;
        private ScoreManager _scoreManager;
        
        #endregion

        #region MonoBehaviour Callbacks
        
        protected override void Awake()
        {
            base.Awake();
            InitializeComponent();
        }

        private void OnEnable()
        {
            GameEventHandler.OnGameWin += GameWin;
            GameEventHandler.OnGameOver += GameOver;
            GameEventHandler.OnContinueStage += ContinueStage;
        }
        
        private void OnDisable()
        {
            GameEventHandler.OnGameWin -= GameWin;
            GameEventHandler.OnGameOver -= GameOver;
            GameEventHandler.OnContinueStage += ContinueStage;
        }
        
        private void Start()
        {
            IsGameStart = true;
        }

        #endregion

        #region Labirin Kata Callbacks
        
        //-- Initialization
        private void InitializeComponent()
        {
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
            
            _letterManager = GameObject.FindGameObjectWithTag("LetterManager").GetComponent<LetterManager>();
            _scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        }
        
        #endregion
        
        #region Game State Callbacks
        
        //-- Core Functionality
        private void GameWin()
        {
            _playerController.StopMovement();
            _timeController.IsTimerStart = false;
            IsGameStart = false;
            
            _scoreManager.RateLevelScore();
            gameWinPanelUI.SetActive(true);
        }
        
        private void GameOver()
        {
            _playerController.StopMovement();
            _timeController.IsTimerStart = false;
            IsGameStart = false;
            
            gameOverPanelUI.SetActive(true);
            Time.timeScale = 0;
        }
        
        private void ContinueStage()
        {
            StartCoroutine(ContinueStageRoutine());
        }
        
        private IEnumerator ContinueStageRoutine()
        {
            _playerController.StopMovement();
            _timeController.IsTimerStart = false;
            _timeController.SetLatestTimer();
            
            SceneTransitionManager.Instance.FadeOut();
            
            yield return new WaitForSeconds(FADE_OUT_DELAY);
            StageManager.Instance.LoadNextStage();
            _letterManager.InitializeLetterEvent();
            _timeController.InitializeTimer(); 
            _playerController.transform.position = Vector2.zero;
            
            yield return new WaitForSeconds(LOAD_STAGE_DELAY);
            SceneTransitionManager.Instance.FadeIn();
            
            yield return new WaitForSeconds(FADE_IN_DELAY);
            _playerController.StartMovement();
            _timeController.IsTimerStart = true;
            IsGameStart = true;
            notificationStagePanelUI.SetActive(true);
        }
        
        #endregion
        
    }
}