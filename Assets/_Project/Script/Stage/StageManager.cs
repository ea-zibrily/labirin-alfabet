using System;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using Alphabet.Enum;
using Alphabet.Database;
using Alphabet.Item;
using Alphabet.DesignPattern.Singleton;

namespace Alphabet.Stage
{
    public class StageManager : MonoSingleton<StageManager>
    {
        #region Fields & Properties
        
        [Header("Settings")] 
        public Level CurrentLevelList;
        public Level NextLevelList;
        [ReadOnly] public StageNum CurrentStageList;

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
            var letter = LetterHelper.GetLetterManagerObject();
            _letterManager = letter.GetComponent<LetterManager>();
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
            _letterManager.SpawnLetter();
        }

        public void SaveClearedLevel()
        {
            var currentLevel = CurrentLevelList.ToString();
            var nextLevel = NextLevelList.ToString();

            GameDatabase.Instance.SaveLevelClear(currentLevel, true);
            GameDatabase.Instance.SaveLevelUnlocked(nextLevel, true);
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