using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Alphabet.Enum;

namespace Alphabet.Stage
{
    public class StageMarker : MonoBehaviour
    {
        #region Variable

        [Header("UI")] 
        [SerializeField] private TextMeshProUGUI stageTextUI;
        
        private string _currentLevel;
        private string _currentStage;
        
        #endregion

        #region MonoBehavior Callbacks
        
        private void Start()
        {
            SetStageNotification();
        }
        
        #endregion

        #region Methods
        
        // !-- Core Functionality
        public void SetStageNotification()
        {
            var levelIndex = StageHelper.GetStageIntValue(StageManager.Instance.CurrentLevelList);
            _currentLevel = StageHelper.GetStageStringValue(levelIndex);
            _currentStage = GetCurrentStage(StageManager.Instance.CurrentStageList);

            stageTextUI.text = _currentLevel.ToUpper() + " - " + _currentStage;
        }
        
        // !-- Helpers/Utilities
        private string GetCurrentStage(StageNum stage)
        {
            return stage switch
            {
                StageNum.Stage_1 => "1",
                StageNum.Stage_2 => "2",
                StageNum.Stage_3 => "3",
                _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, null)
            };
        }

        #endregion
    }
}