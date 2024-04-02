using System;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using Alphabet.Enum;
using Alphabet.Item;
using Alphabet.Database;
using Alphabet.DesignPattern.Singleton;

namespace Alphabet.Stage
{
    public class StageManager : MonoSingleton<StageManager>
    {
        #region Fields & Properties
        
        [Header("Settings")] 
        public Level CurrentLevelList;
        [ReadOnly] public StageNum CurrentStageList;

        [Header("Stage")]
        [SerializeField] private GameObject[] stageObjects;
        [SerializeField] [ReadOnly] private int currentStageIndex;
        
        public int CurrentStageIndex => currentStageIndex;
        public int StageCount => stageObjects.Length;
        
        // Reference
        public LetterManager LetterManager { get; private set; }
        
        #endregion
        
        #region MonoBehaviour Callbacks

        protected override void Awake()
        {
            var letter = LetterHelper.GetLetterManagerObject();
            LetterManager = letter.GetComponent<LetterManager>();
        }
        
        private void Start()
        {
            InitializeLeveStage();
        }
        
        #endregion

        #region Methods
        
        // !-- Initialization
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
                    CurrentStageList = StageNum.Stage_1;
                    continue;
                }
                
                stageObjects[i].SetActive(false);
            }
        }
        
        // !-- Core Functionality
        public void InitializeNewStage()
        {
            LoadNextStage();
            LetterManager.SpawnLetter();
        }

        public void SaveClearStage()
        {
            var currentLevel = CurrentLevelList.ToString();
            SaveClearIndex(currentLevel);
            GameDatabase.Instance.SaveLevelClear(currentLevel, true);
        }

        private void SaveClearIndex(string levelName)
        {
            // Checker
            var levelCondition = GameDatabase.Instance.LoadLevelConditions(levelName);
            if (levelCondition) return;
            
            var levelIndex = (int)System.Enum.Parse(typeof(Level), levelName);
            levelIndex = levelIndex < System.Enum.GetNames(typeof(Level)).Length - 1 ? levelIndex + 1 : 0;
            GameDatabase.Instance.SaveLevelClearIndex(levelIndex);
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
        
        // !-- Helper/Utilities
        public bool CheckCanContinueStage()
        {
            return currentStageIndex < stageObjects.Length - 1;
        }
        
        private StageNum GetCurrentStage(int index)
        {
            return index switch
            {
                0 => StageNum.Stage_1,
                1 => StageNum.Stage_2,
                2 => StageNum.Stage_3,
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };
        }
        
        #endregion
    }
}