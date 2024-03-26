using UnityEngine;
using UnityEngine.UI;

namespace Alphabet.UI
{
    public class GamePauseController : GameUIBase
    {
        #region Fields & Properties

        [Header("Pause")] 
        [SerializeField] private Button resumeButtonUI;
        [SerializeField] private GameObject pausePanelUI;
        [SerializeField] private GameObject controllerCanvas;
        
        private Button pauseButtonUI;
        
        #endregion

        #region Methods

        // !-- Initialization
        protected override void InitializeOnAwake()
        {
            base.InitializeOnAwake();
            pauseButtonUI = GetComponent<Button>();
        }

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
            controllerCanvas.SetActive(false);
            
            Time.timeScale = 0;
        }
        
        private void OnResumeButton()
        {
            pausePanelUI.SetActive(false);
            controllerCanvas.SetActive(true);
            
            Time.timeScale = 1;
        }
        
        #endregion
    }
}