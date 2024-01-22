using System;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Enum;
using LabirinKata.DesignPattern.Singleton;

namespace LabirinKata.Stage
{
    public class StageManager : MonoSingleton<StageManager>
    {
        #region Variable
        
        [Header("Settings")] 
        public LevelList CurrentLevelList;
        [ReadOnly] public StageList CurrentStageList;

        [Header("Stage")]
        [SerializeField] private GameObject[] stageObjects;

        private int _currentStageIndex;
        public int CurrentStageIndex => _currentStageIndex;
        
        #endregion
        
        #region MonoBehaviour Callbacks

        private void Start()
        {
            InitializeLeveStage();
        }
        
        #endregion

        #region Labirin Kata Callbacks
        
        //-- Initialization
        private void InitializeLeveStage()
        {
            if (stageObjects == null)
            {
                Debug.LogError("stage object null brok");
                return;
            }

            for (var i = 0; i < stageObjects.Length; i++)
            {
                if (i is 0)
                {
                    stageObjects[i].SetActive(true);
                    _currentStageIndex = i;
                    CurrentStageList = StageList.Stage_1;
                    continue;
                }
                
                stageObjects[i].SetActive(false);
            }
        }
        
        //-- Core Functionality
        public void LoadNextStage()
        {
            if (!CheckCanContinueStage()) return;
            
            for (var i = 0; i < stageObjects.Length; i++)
            {
                if (!stageObjects[i].activeSelf) continue;
                
                stageObjects[i].SetActive(false);
                _currentStageIndex = i;
                break;
            }
            
            _currentStageIndex += 1;
            CurrentStageList = GetCurrentStage(_currentStageIndex);
            stageObjects[_currentStageIndex].SetActive(true);
        }
        
        //-- Helper/Utilities
        public bool CheckCanContinueStage()
        {
            return _currentStageIndex < stageObjects.Length - 1;
        }
        
        private StageList GetCurrentStage(int index)
        {
            return index switch
            {
                0 => StageList.Stage_1,
                1 => StageList.Stage_2,
                2 => StageList.Stage_3,
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }
        
        #endregion
    }
}