using System;
using LabirinKata.Enum;
using TMPro;
using UnityEngine;

namespace LabirinKata.Stage
{
    public class StagePanelController : MonoBehaviour
    {
        #region Variable

        [Header("UI")] 
        [SerializeField] private TextMeshProUGUI levelTextUI;
        [SerializeField] private TextMeshProUGUI stageTextUI;

        public event Action OnAnimationStageDone;
        
        private string _currentLevel;
        private string _currentStage;
        
        [Header("Reference")] 
        private StageManager _stageManager;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        }

        private void OnEnable()
        {
            OnAnimationStageDone += InitializeStagePanel;
        }

        private void OnDisable()
        {
            OnAnimationStageDone -= InitializeStagePanel;
        }

        private void Start()
        {
            InitializeStagePanel();
        }

        #endregion

        #region Labirin Kata Callbacks

        //-- Initialization
        private void InitializeStagePanel()
        {
            levelTextUI.text = "";
            stageTextUI.text = "";
            gameObject.SetActive(false);
        }
        
        //-- Core Functionality
        // TODO: Call when continue stage
        public void ActivateStagePanel()
        {
            gameObject.SetActive(true);
            
            _currentLevel = GetCurrentLevel(_stageManager.CurrentLevelList);
            _currentStage = GetCurrentStage(_stageManager.CurrentStageList);
            levelTextUI.text = _currentLevel;
            stageTextUI.text = _currentStage;
        }
        
        public void DeactivateStagePanel() => OnAnimationStageDone?.Invoke();
        
        //-- Helpers/Utilities
        private string GetCurrentLevel(LevelList level)
        {
            var currentLevel = "";
            switch (level)
            {
                case LevelList.Level_01:
                    currentLevel = "Level 1";
                    break;
                case LevelList.Level_02:
                    currentLevel = "Level 2";
                    break;
                case LevelList.Level_03:
                    currentLevel = "Level 3";
                    break;
                case LevelList.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            return currentLevel;
        }
        
        private string GetCurrentStage(StageList stage)
        {
            var currentStage = "";
            switch (stage)
            {
                case StageList.Stage_1:
                    currentStage = "Stage 1";
                    break;
                case StageList.Stage_2:
                    currentStage = "Stage 2";
                    break;
                case StageList.Stage_3:
                    currentStage = "Stage e";
                    break;
                case StageList.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stage), stage, null);
            }

            return currentStage;
        }

        #endregion
        
    }
}