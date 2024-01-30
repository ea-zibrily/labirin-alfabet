using System;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Database;
using LabirinKata.Item.Letter;
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
        [SerializeField] [ReadOnly] private int currentStageIndex;
        
        public int CurrentStageIndex => currentStageIndex;
        public int StageCount => stageObjects.Length;
        
        [Header("Reference")]
        private LetterManager _letterManager;
        public LetterManager LetterManager => _letterManager;
        
        #endregion
        
        #region MonoBehaviour Callbacks

        protected override void Awake()
        {
            _letterManager = GameObject.FindGameObjectWithTag("LetterManager").GetComponentInChildren<LetterManager>();
        }
        
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
                    currentStageIndex = i;
                    CurrentStageList = StageList.Stage_1;
                    continue;
                }
                
                stageObjects[i].SetActive(false);
            }
        }
        
        //-- Core Functionality
        public void InitializeNewStage()
        {
            LoadNextStage();
            _letterManager.SpawnLetter();
        }

        public void SaveClearedLevel()
        {
            var currentLevel = CurrentLevelList.ToString();
            GameDatabase.Instance.SaveLevelConditions(currentLevel, true);
        }
        
        private void LoadNextStage()
        {
            if (!CheckCanContinueStage()) return;
            
            for (var i = 0; i < stageObjects.Length; i++)
            {
                if (!stageObjects[i].activeSelf) continue;
                
                stageObjects[i].SetActive(false);
                currentStageIndex = i;
                break;
            }
            
            currentStageIndex += 1;
            CurrentStageList = GetCurrentStage(currentStageIndex);
            stageObjects[currentStageIndex].SetActive(true);
        }
        
        //-- Helper/Utilities
        public bool CheckCanContinueStage()
        {
            return currentStageIndex < stageObjects.Length - 1;
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