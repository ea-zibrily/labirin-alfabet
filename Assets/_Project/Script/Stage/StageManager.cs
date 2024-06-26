using System;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using Alphabet.Enum;
using Alphabet.Letter;
using Alphabet.Database;
using Alphabet.DesignPattern.Singleton;

namespace Alphabet.Stage
{
    public class StageManager : MonoSingleton<StageManager>
    {
        #region Fields & Properties
        
        [Header("Settings")] 
        public StageName CurrentStage;
        [ReadOnly] public StageNum CurrentStageNum;

        [Header("Stage")]
        [SerializeField] private GameObject[] stageObjects;
        
        public int CurrentStageIndex { get; private set; }
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
                Debug.LogError("stage object null breks");
                return;
            }

            for (var i = 0; i < stageObjects.Length; i++)
            {
                var isStageOne = i is 0;

                stageObjects[i].SetActive(isStageOne);
                if (isStageOne)
                {
                    CurrentStageIndex = i;
                    CurrentStageNum = StageNum.Stage_1;
                }
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
            var currentLevel = CurrentStage.ToString();
            SaveClearIndex(currentLevel);
            GameDatabase.Instance.SaveLevelClear(currentLevel, true);
        }

        private void SaveClearIndex(string levelName)
        {
            // Checker
            var levelCondition = GameDatabase.Instance.LoadLevelConditions(levelName);
            if (levelCondition) return;
            
            var levelIndex = (int)System.Enum.Parse(typeof(StageName), levelName);
            levelIndex = levelIndex < System.Enum.GetNames(typeof(StageName)).Length - 1 ? levelIndex + 1 : 0;
            GameDatabase.Instance.SaveLevelClearIndex(levelIndex);
        }

        private void LoadNextStage()
        {
            if (!CheckCanContinueStage()) return;
            
            for (var i = 0; i < stageObjects.Length; i++)
            {
                if (!stageObjects[i].activeSelf) continue;
                
                stageObjects[i].SetActive(false);
                CurrentStageIndex = i;
                break;
            }
            
            CurrentStageIndex += 1;
            CurrentStageNum = GetCurrentStage(CurrentStageIndex);
            stageObjects[CurrentStageIndex].SetActive(true);
        }
        
        // !-- Helper/Utilities
        public bool CheckCanContinueStage()
        {
            return CurrentStageIndex < stageObjects.Length - 1;
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