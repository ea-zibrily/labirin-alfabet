using System;
using System.Collections;
using LabirinKata.Enum;
using TMPro;
using UnityEngine;

namespace LabirinKata.Stage
{
    public class StagePanelController : MonoBehaviour
    {
        #region Variable

        [Header("UI")] 
        [SerializeField] private float activateDelayTime;
        [SerializeField] private TextMeshProUGUI levelTextUI;
        [SerializeField] private TextMeshProUGUI stageTextUI;
        
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
            ActivateStagePanel();
        }
        
        #endregion

        #region Labirin Kata Callbacks
        
        //-- Core Functionality
        private void ActivateStagePanel() => StartCoroutine(ActivateStagePanelRoutine());
        
        private IEnumerator ActivateStagePanelRoutine()
        {
            SetCurrentLevel(_stageManager.CurrentLevelList);
            SetCurrentStage(_stageManager.CurrentStageList);
            levelTextUI.text = _currentLevel;
            stageTextUI.text = _currentStage;
            
            yield return new WaitForSeconds(activateDelayTime);
            gameObject.SetActive(true);
        }
        
        private void DeactivateStagePanel()
        {
            levelTextUI.text = "";
            stageTextUI.text = "";
            levelTextUI.color = new Color(0, 0, 0, 1);
            stageTextUI.color = new Color(0, 0, 0, 1);
            
            gameObject.SetActive(false);
        }
        
        //-- Helpers/Utilities
        private string GetCurrentLevel(LevelList level)
        {
            return level switch
            {
                LevelList.Level_01 => "Level 1",
                LevelList.Level_02 => "Level 2",
                LevelList.Level_03 => "Level 3",
                LevelList.None => " ",
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
        }
        
        private void SetCurrentLevel(LevelList level) => _currentLevel = GetCurrentLevel(level);
        
        private string GetCurrentStage(StageList stage)
        {
            return stage switch
            {
                StageList.Stage_1 => "Stage 1",
                StageList.Stage_2 => "Stage 2",
                StageList.Stage_3 => "Stage 3",
                StageList.None => " ",
                _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, null)
            };
        }

        private void SetCurrentStage(StageList stage) => _currentStage = GetCurrentStage(stage);

        #endregion

    }
}