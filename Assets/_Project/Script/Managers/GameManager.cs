using System;
using System.Collections;
using UnityEngine;
using Alphabet.Stage;
using Alphabet.Entities.Player;
using Alphabet.Gameplay.Controller;
using Alphabet.Gameplay.EventHandler;
using Alphabet.DesignPattern.Singleton;

namespace Alphabet.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Constant Variable
        
        //-- Load stage time delay
        private const float FADE_OUT_DELAY = 2.5f;
        private const float LOAD_STAGE_DELAY = 1.2f;
        private const float FADE_IN_DELAY = 1.5f;
        
        #endregion
        
        #region Fields & Properties
        
        [Header("UI")] 
        [SerializeField] private GameObject gameWinPanelUI;
        [SerializeField] private GameObject gameOverPanelUI;

        public bool IsGameStart { get; private set; }
        
        [Header("Reference")] 
        private PlayerController _playerController;
        private TimeController _timeController;
        private TutorialController _tutorialController;
        private StageMarker _stageMarker;
        
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
            GameEventHandler.OnContinueStage -= ContinueStage;
        }

        private void Start()
        {
            IsGameStart = true;
        }

        #endregion

        #region Labirin Kata Callbacks
        
        // !-- Initialization
        private void InitializeComponent()
        {
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _tutorialController = GameObject.Find("TutorialController").GetComponent<TutorialController>();
            _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
            _stageMarker = GameObject.Find("StageMarker").GetComponent<StageMarker>();
        }
        
        #endregion
        
        #region Game State Callbacks
        
        // !-- Core Functionality
        private void GameWin()
        {
            IsGameStart = false;
            _timeController.IsTimerStart = false;
            _playerController.StopMovement();
            
            StageManager.Instance.SaveClearStage();
            StageManager.Instance.LetterManager.SaveUnlockedLetters();
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
            StageManager.Instance.InitializeNewStage();
            _tutorialController.CallTutorial();
            _stageMarker.SetStageNotification();
            _timeController.InitializeTimer();
            PlayerSpawner.SpawnPlayerEvent();
            
            yield return new WaitForSeconds(LOAD_STAGE_DELAY);
            SceneTransitionManager.Instance.FadeIn();
            
            yield return new WaitForSeconds(FADE_IN_DELAY);
            _playerController.StartMovement();
            _timeController.IsTimerStart = true;
            IsGameStart = true;
        }
        
        #endregion
        
    }
}