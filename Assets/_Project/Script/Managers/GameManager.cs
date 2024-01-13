using System;
using UnityEngine;
using CariHuruf.Enum;
using CariHuruf.Gameplay.Controller;
using CariHuruf.DesignPattern.Singleton;

namespace CariHuruf.Entities.Item
{
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Variable

        [Header("Settings")] 
        public Level CurrentLevel;
        public Stage CurrentStage;

        private string _saveKey;

        [Header("Reference")] 
        [SerializeField] private TimeController timeController;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            SetTimeKey();
        }

        #endregion

        #region Load and Save Callbacks
        
        public float GetLatestTime()
        {
            var lastTime = PlayerPrefs.GetFloat(_saveKey);
            return lastTime;
        }
        
        private void SetTimeKey()
        {
            _saveKey = CurrentLevel + "_" + CurrentStage;
            Debug.LogWarning(_saveKey);
        }
        
        private void SaveLatestCurrentTime()
        {
            if (PlayerPrefs.HasKey(_saveKey))
            {
                PlayerPrefs.SetFloat(_saveKey, timeController.CurrentTime);
            }
        }
        
        #endregion
        
    }
}