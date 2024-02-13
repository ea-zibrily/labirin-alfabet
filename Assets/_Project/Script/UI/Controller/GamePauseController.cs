using UnityEngine;
using UnityEngine.UI;

namespace LabirinKata.UI
{
    public class GamePauseController : GameUIBase
    {
        #region Fields & Properties

        [Header("Pause")] 
        [SerializeField] private Button pauseButtonUI;
        [SerializeField] private Button resumeButtonUI;
        [SerializeField] private GameObject pausePanelUI;
        [SerializeField] private GameObject touchCanvas;
        
        #endregion

        #region Labirin Kata Callbacks
        
        // !-- Initialization
        protected override void InitializeOnStart()
        {
            base.InitializeOnStart();
            pausePanelUI.SetActive(false);
            
            pauseButtonUI.onClick.AddListener(OnPauseButton);
            resumeButtonUI.onClick.AddListener(OnResumeButton);
        }
        
        // !-- Core Functionality
        private void OnPauseButton()
        {
            pausePanelUI.SetActive(true);
            touchCanvas.SetActive(false);
            
            Time.timeScale = 0;
        }
        
        private void OnResumeButton()
        {
            pausePanelUI.SetActive(false);
            touchCanvas.SetActive(true);
            
            Time.timeScale = 1;
        }
        
        #endregion
    }
}