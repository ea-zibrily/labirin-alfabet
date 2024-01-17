using System.Collections;
using UnityEngine;
using LabirinKata.Stage;
using LabirinKata.Entities.Player;
using LabirinKata.Gameplay.Controller;
using LabirinKata.Gameplay.EventHandler;
using LabirinKata.DesignPattern.Singleton;

namespace LabirinKata.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Variable

        [Header("UI")] 
        [SerializeField] private GameObject gameWinPanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject notificationStagePanel;
        
        [Header("Settings")] 
        private string _saveKey;
        
        [Header("Reference")] 
        private PlayerController _playerController;
        private TimeController _timeController;
        
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
        
        #endregion

        #region Labirin Kata Callbacks

        //-- Initialization
        private void InitializeComponent()
        {
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        }
        
        #endregion
        
        #region Game State Callbacks
        
        //-- Core Functionality
        private void GameWin()
        {
            _playerController.StopMovement();
            gameWinPanel.SetActive(true);
            Time.timeScale = 0;
        }
        
        private void GameOver()
        {
            _playerController.StopMovement();
            gameOverPanel.SetActive(true);
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
            SceneTransitionManager.Instance.FadeIn();
            
            yield return new WaitForSeconds(0.5f);
            StageManager.Instance.LoadNextStage();
            _timeController.InitializeTimer();
            SceneTransitionManager.Instance.FadeOut();
            
            yield return new WaitForSeconds(1f);
            _playerController.StartMovement();
            _timeController.IsTimerStart = true;
            
            yield return new WaitForSeconds(2.5f);
            notificationStagePanel.SetActive(true);
        }
        
        #endregion
        
    }
}