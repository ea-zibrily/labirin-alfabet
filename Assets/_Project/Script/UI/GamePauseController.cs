using LabirinKata.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace LabirinKata.UI
{
    public class GamePauseController : GameUIBase
    {
        #region Variable

        [SerializeField] private Button resumeButtonUI;

        [Header("Pause")] 
        [SerializeField] private Button pauseButtonUI;
        [SerializeField] private GameObject pausePanelUI;
        
        #endregion

        #region Labirin Kata Callbacks

        //-- Initialization
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            
            pausePanelUI.SetActive(false);
            
            pauseButtonUI.onClick.AddListener(OnPauseButton);
            resumeButtonUI.onClick.AddListener(OnResumeButton);
        }
        
        //-- Core Functionality
        private void OnPauseButton()
        {
            pausePanelUI.SetActive(true);
            GameManager.Instance.IsGameStart = false;
            GameManager.Instance.PlayerController.StopMovement();
            
            Time.timeScale = 0;
        }
        
        private void OnResumeButton()
        {
            pausePanelUI.SetActive(false);
            GameManager.Instance.IsGameStart = true;
            GameManager.Instance.PlayerController.StartMovement();
            
            Time.timeScale = 1;
        }

        #endregion
    }
}