using System;
using UnityEngine;
using CariHuruf.Enum;

namespace CariHuruf.Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Variable
        
        [Header("Settings")] 
        public Level CurrentLevel;
        public Stage CurrentStage;
        [SerializeField] private bool canContinueStage;
        
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

        #region CariHuruf Callbacks
        
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
        
        #endregion
    }
}