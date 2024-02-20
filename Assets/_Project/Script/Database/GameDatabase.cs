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

        //-- Main Database
        private Dictionary<int, bool> _letterConditions;
        private Dictionary<string, bool> _levelConditions;
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
            if (_letterConditions != null && _levelConditions != null && _isLevelUnlocked != null) return;
            
            _letterConditions = InitializeLetterConditions();
            _levelConditions = InitializeLevelConditions();
            _isLevelUnlocked = InitializeLevelUnlocked();
        }
        
        private Dictionary<int, bool> InitializeLetterConditions()
        {
            var letterConditions = new Dictionary<int, bool>();
    
            for (var i = 0; i < LETTER_COUNT; i++)
            {
                var letterKey = i + 1;
                letterConditions.Add(letterKey, false);
            }
            return letterConditions;
        }

        private Dictionary<string, bool> InitializeLevelConditions()
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
        public void SaveLetterConditions(int letterId, bool value)
        {
            if (_letterConditions.ContainsKey(letterId))
            {
                _letterConditions[letterId] = value;
            }
        }
        
        public bool LoadLetterConditions(int letterId)
        {
            return _letterConditions[letterId];
        }
        
        public void SaveLevelConditions(string levelName, bool value)
        {
            if (_levelConditions.ContainsKey(levelName))
            {
                _levelConditions[levelName] = value;
            }
        }
        
        public bool LoadLevelConditions(string levelName)
        {
            return _levelConditions[levelName];
        }
        
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