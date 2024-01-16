using LabirinKata.Enum;
using UnityEngine;

namespace LabirinKata.Stage
{
    public class StageManager : MonoBehaviour
    {
        #region Variable
        
        [Header("Settings")] 
        public LevelList CurrentLevelList;
        public StageList CurrentStageList;
        [SerializeField] private int maxStage;
        [SerializeField] private bool canContinueStage;
        
        [Header("Stage")]
        [Tooltip("Isi dengan prefabs stage sesuai dengan stage level")]
        [SerializeField] private GameObject[] stageObjects;
        [SerializeField] private bool isStageStart;
        
        public bool IsStageStart
        {
            get => isStageStart;
            set => isStageStart = value;
        }
        
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
        }
        
        //-- Core Functionality
        // TODO: Call when continue stage
        public void LoadNextStage()
        {
            if (!canContinueStage || maxStage < 2) return;

            var currentStageIndex = 0;
            for (var i = 0; i < stageObjects.Length; i++)
            {
                if (!stageObjects[i].activeSelf) continue;
                
                stageObjects[i].SetActive(false);
                currentStageIndex = i;
                break;
            }
            
            currentStageIndex += 1;
            stageObjects[currentStageIndex].SetActive(true);
        }
        
        #endregion
    }
}