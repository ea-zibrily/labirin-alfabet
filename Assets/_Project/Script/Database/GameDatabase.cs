using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LabirinKata.Enum;
using LabirinKata.DesignPattern.Singleton;

namespace LabirinKata.Database
{
    public class GameDatabase : MonoDDOL<GameDatabase>
    {
        #region Fields & Properties
        
        //-- Game Database
        private Dictionary<int, bool> _isLetterCollected;
        private Dictionary<string, bool> _isLevelClear;
        private Dictionary<string, bool> _isLevelUnlocked;
        
        //-- Constant Variable
        public const int LETTER_COUNT = 26;
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void OnEnable()
        {
            InitializeData();
        }
        
        #endregion

        #region Labirin Kata Callbacks

        // !-- Initialization
        private void InitializeData()
        {
            if (_isLetterCollected != null && _isLevelClear != null && _isLevelUnlocked != null) return;
            
            _isLetterCollected = InitializeLetterCollected();
            _isLevelClear = InitializeLevelClear();
            _isLevelUnlocked = InitializeLevelUnlocked();
        }
        
        private Dictionary<int, bool> InitializeLetterCollected()
        {
            var letterConditions = new Dictionary<int, bool>();
    
            for (var i = 0; i < LETTER_COUNT; i++)
            {
                var letterKey = i + 1;
                letterConditions.Add(letterKey, false);
            }
            return letterConditions;
        }

        private Dictionary<string, bool> InitializeLevelClear()
        {
            var levelConditions = new Dictionary<string, bool>();
    
            foreach (Level level in System.Enum.GetValues(typeof(Level)))
            {
                var key = level.ToString();
                levelConditions.Add(key, false);
            }
            return levelConditions;
        }
        
        private Dictionary<string, bool> InitializeLevelUnlocked()
        {
            var isLevelUnlocked = new Dictionary<string, bool>();
    
            foreach (Level level in System.Enum.GetValues(typeof(Level)))
            {
                var key = level.ToString();
                var value = key is "Cave";
                
                isLevelUnlocked.Add(key, value);
            }
            return isLevelUnlocked;
        }

        // !-- Core Functionality

        // * Letter Collected
        public void SaveLetterCollected(int letterId, bool value)
        {
            if (_isLetterCollected.ContainsKey(letterId))
            {
                _isLetterCollected[letterId] = value;
            }
        }
        
        public bool LoadLetterConditions(int letterId)
        {
            return _isLetterCollected[letterId];
        }
        
        // * Level Clear
        public void SaveLevelClear(string levelName, bool value)
        {
            if (_isLevelClear.ContainsKey(levelName))
            {
                _isLevelClear[levelName] = value;
            }
        }
        
        public bool LoadLevelConditions(string levelName)
        {
            return _isLevelClear[levelName];
        }
        
        // * Level Unlock
        public void SaveLevelUnlocked(string levelName, bool value)
        {
            if (_isLevelUnlocked.ContainsKey(levelName))
            {
                _isLevelUnlocked[levelName] = value;
            }
        }

        public bool LoadLevelUnlocked(string levelName)
        {
            return _isLevelUnlocked[levelName];
        }

        #endregion
    }
}