﻿using System;
using System.Collections;
using UnityEngine;
using TMPro;
using LabirinKata.Enum;

namespace LabirinKata.Stage
{
    public class StageMarker : MonoBehaviour
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
            _currentLevel = StageManager.Instance.CurrentLevelList.ToString();
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