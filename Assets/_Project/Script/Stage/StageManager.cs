using LabirinKata.DesignPattern.Singleton;
using LabirinKata.Enum;
using UnityEngine;

namespace LabirinKata.Stage
{
    public class StageManager : MonoSingleton<StageManager>
    {
        #region Variable
        
        [Header("Settings")] 
        public LevelList CurrentLevelList;
        public StageList CurrentStageList;
        
        [SerializeField] private bool canContinueStage;
        public bool CanContinueStage => canContinueStage;

        [Header("Stage")]
        [Tooltip("Isi dengan prefabs stage sesuai dengan stage level")]
        [SerializeField] private GameObject[] stageObjects;
        
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
                stageObjects[i].SetActive(i is 0);
            }

            CurrentStageList = StageList.Stage_1;
        }
        
        //-- Core Functionality
        public void LoadNextStage()
        {
            if (!canContinueStage) return;

            var currentStageIndex = 0;
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
        private StageList GetCurrentStage(int index)
        {
            var stage = index switch
            {
                0 => StageList.Stage_1,
                1 => StageList.Stage_2,
                2 => StageList.Stage_3,
                _ => StageList.None
            };

            return stage;
        }
        
        #endregion
    }
}