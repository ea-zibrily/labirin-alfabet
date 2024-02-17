using System;
using System.Collections;
using UnityEngine;
using TMPro;
using LabirinKata.Enum;

namespace LabirinKata.Stage
{
    public class StageNotificationController : MonoBehaviour
    {
        #region Variable

        [Header("UI")] 
        [SerializeField] private TextMeshProUGUI stageTextUI;
        
        private string _currentLevel;
        private string _currentStage;

        #endregion

        #region Labirin Kata Callbacks
        
        // !-- Core Functionality
        public void SetStageNotification()
        {
            SetCurrentLevel(StageManager.Instance.CurrentLevelList);
            SetCurrentStage(StageManager.Instance.CurrentStageList);
            stageTextUI.text = _currentLevel.ToUpper() + " - " + _currentStage;
        }
        
        // !-- Helpers/Utilities
        private string GetCurrentLevel(LevelList level)
        {
            return level switch
            {
                LevelList.Level_01 => "Cave",
                LevelList.Level_02 => "Forest",
                LevelList.Level_03 => "Ruins",
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
        }
        
        private void SetCurrentLevel(LevelList level) => _currentLevel = GetCurrentLevel(level);
        
        private string GetCurrentStage(StageList stage)
        {
            return stage switch
            {
                StageList.Stage_1 => "1",
                StageList.Stage_2 => "2",
                StageList.Stage_3 => "3",
                _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, null)
            };
        }

        private void SetCurrentStage(StageList stage) => _currentStage = GetCurrentStage(stage);

        #endregion
    }
}