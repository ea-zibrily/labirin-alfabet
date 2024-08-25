using Alphabet.Enum;
using Alphabet.Managers;
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
        private PauseEventHandler _pauseEventHandler;
        
        #endregion

        #region Methods

        // !-- Initialization
        protected override void InitializeOnAwake()
        {
            base.InitializeOnAwake();
            pauseButtonUI = GetComponent<Button>();
            _pauseEventHandler = pausePanelUI.GetComponent<PauseEventHandler>();
        }

        protected override void InitializeOnStart()
        {
            base.InitializeOnStart();
            pausePanelUI.SetActive(false);
            
            pauseButtonUI.onClick.AddListener(OnPauseButton);
            resumeButtonUI.onClick.AddListener(OnResumeButton);
        }

        private void OnEnable()
        {
            _pauseEventHandler.OnGamePause += () => Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            _pauseEventHandler.OnGamePause -= () => Time.timeScale = 0f;
        }
        
        // !-- Core Functionality
        private void OnPauseButton()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            pausePanelUI.SetActive(true);
            controllerCanvas.SetActive(false);
        }
        
        private void OnResumeButton()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            pausePanelUI.SetActive(false);
            controllerCanvas.SetActive(true);
            
            Time.timeScale = 1;
        }
        
        #endregion
    }
}