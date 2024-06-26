using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alphabet.Enum;
using Alphabet.DesignPattern.Singleton;

namespace Alphabet.Database
{
    public class GameDatabase : MonoDDOL<GameDatabase>
    {
        #region Fields & Properties
        
        //-- Game Database
        private Dictionary<int, bool> _isLetterCollected;
        private Dictionary<string, bool> _isLevelClear;
        private int _levelClearIndex;
        
        // public bool IsAnimateUnlock => _levelClearIndex > 0;
        
        //-- Constant Variable
        public const int LETTER_COUNT = 26;
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void OnEnable()
        {
            InitializeData();
        }
        
        #endregion

        #region Methods

        // !-- Initialization
        private void InitializeData()
        {
            if (_isLetterCollected != null && _isLevelClear != null) return;
            
            _isLetterCollected = InitializeLetterCollected();
            _isLevelClear = InitializeLevelClear();
            _levelClearIndex = 0;
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
    
            foreach (StageName level in System.Enum.GetValues(typeof(StageName)))
            {
                var key = level.ToString();
                levelConditions.Add(key, false);
            }
            return levelConditions;
        }
        
        // !-- Core Functionality

        // Letter Collected
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
        
        // Level Clear
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

        // Level Clear Index
        public void SaveLevelClearIndex(int value) => _levelClearIndex = value;
        public void ResetLevelClearIndex() => _levelClearIndex = 0;
        public int LoadLevelClearIndex() 
        {
            return _levelClearIndex;
        }

        #endregion
    }
}