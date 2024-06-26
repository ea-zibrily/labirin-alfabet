using System;
using System.Collections;
using UnityEngine;
using Alphabet.UI;
using Alphabet.Enum;
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
        
        //Load stage time delay
        private const float FADE_OUT_DELAY = 1f;
        private const float LOAD_STAGE_DELAY = 1.25f;
        
        #endregion
        
        #region Fields & Properties
        
        [Header("UI")] 
        [SerializeField] private GameObject gameWinPanelUI;
        [SerializeField] private GameObject gameOverPanelUI;

        public bool IsGameStart { get; private set; }
        
        [Header("Reference")] 
        private PlayerController _playerController;
        private PlayerManager _playerManager;
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
            GameEventHandler.OnContinueStage -= ContinueStage;
        }

        #endregion

        #region Labirin Kata Callbacks
        
        // !-- Initialization
        private void InitializeComponent()
        {
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _playerManager = _playerController.GetComponentInChildren<PlayerManager>();
            _tutorialController = GameObject.Find("TutorialController").GetComponent<TutorialController>();
            _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
            _stageMarker = GameObject.Find("StageMarker").GetComponent<StageMarker>();
        }
        
        #endregion
        
        #region Game State Callbacks
        
        // !-- Core Functionality
        private void GameStart()
        {
            IsGameStart = true;
        }

        private void GameWin()
        {
            // Win Game
            IsGameStart = false;
            _timeController.IsTimerStart = false;
            _playerController.StopMovement();
            _playerManager.ResetLetter();
            
            StageManager.Instance.SaveClearStage();
            StageManager.Instance.LetterManager.SaveUnlockedLetters();
            gameWinPanelUI.SetActive(true);

            // Audio
            AudioController.FadeAudioEvent(isFadeIn: false);
            FindObjectOfType<AudioManager>().PlayAudio(Musics.WinSfx);
        }

        private void GameOver(LoseType loseType)
        {
            // Lose Game
            IsGameStart = false;
            _timeController.IsTimerStart = false;
            _playerController.StopMovement();

            gameOverPanelUI.GetComponent<GameOverController>().SetGameOverInterface(loseType);
            gameOverPanelUI.SetActive(true);

            // Audio
            AudioController.FadeAudioEvent(isFadeIn: false);
            FindObjectOfType<AudioManager>().PlayAudio(Musics.LoseSfx);
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
            _playerManager.ResetLetter();
            
            AudioController.FadeAudioEvent(isFadeIn: false);
            SceneTransitionManager.Instance.FadeOut();
            
            yield return new WaitForSeconds(FADE_OUT_DELAY);
            StageManager.Instance.InitializeNewStage();
            _tutorialController.CallTutorial();
            _stageMarker.TopMarker();
            _timeController.InitializeTimer();
            _playerController.DefaultDirection();
            PlayerSpawner.SpawnPlayerEvent();
            
            yield return new WaitForSeconds(LOAD_STAGE_DELAY);
            AudioController.FadeAudioEvent(isFadeIn: true);
            SceneTransitionManager.Instance.FadeIn();
        }
        
        #endregion
        
    }
}