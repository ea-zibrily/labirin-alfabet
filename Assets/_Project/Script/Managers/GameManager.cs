using LabirinKata.DesignPattern.Singleton;
using LabirinKata.Entities.Player;
using LabirinKata.Gameplay.Controller;
using LabirinKata.Gameplay.EventHandler;
using LabirinKata.Stage;
using UnityEngine;
using UnityEngine.Serialization;

namespace LabirinKata.Managers
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

        [Header("UI")] 
        [SerializeField] private GameObject openingStagePanel;
        [SerializeField] private GameObject gameWinPanel;
        [SerializeField] private GameObject gameOverPanel;
        
        [Header("Settings")] 
        private string _saveKey;
        
        [Header("Reference")] 
        //-- Controller
        private PlayerController _playerController;
        private TimeController _timeController;

        //-- Manager
        private StageManager _stageManager;
        private ScoreManager _scoreManager;
        
        #endregion

        #region MonoBehaviour Callbacks
        
        protected override void Awake()
        {
            base.Awake();
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();

            _stageManager = GameObject.Find("LevelManager").GetComponent<StageManager>();
            _scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
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
            _scoreManager.RateStar();
        }
        
        private void ContinueStage()
        {
            _playerController.StopMovement();
            SceneTransitionManager.Instance.LoadNextScene();
        }
        
        #endregion
        
        #region Load and Save Callbacks
        
        //-- Initialization
        // private string SetScorePrefsKey(LevelState state)
        // {
        //     var currentKey = state switch
        //     {
        //         LevelState.Current => CurrentLevel + "_" + CurrentStage,
        //         LevelState.Previous => PreviousLevel + "_" + PreviousStage,
        //         LevelState.None => "None!",
        //         _ => ""
        //     };
        //     
        //     return currentKey;
        // }
        
        //-- Core Functionality
        // private void SaveCurrentTime()
        // {
        //     _saveKey = SetScorePrefsKey(LevelState.Current);
        //     if (PlayerPrefs.HasKey(_saveKey))
        //     {
        //         PlayerPrefs.SetFloat(_saveKey, _timeController.CurrentTime);
        //     }
        // }
        
        #endregion
    }
}