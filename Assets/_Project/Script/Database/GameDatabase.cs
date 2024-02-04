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
        #region Variable

        //-- Main Database
        private Dictionary<int, bool> _letterConditions;
        private Dictionary<string, bool> _levelConditions ;
        
        //-- Constant Variable
        public const int LETTER_COUNT = 26;
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Start()
        {
            InitializeData();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        private void InitializeData()
        {
            if (_letterConditions != null && _levelConditions != null) return;
    
            _letterConditions = InitializeLetterConditions();
            _levelConditions = InitializeLevelConditions();
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
    
            foreach (LevelList level in System.Enum.GetValues(typeof(LevelList)))
            {
                if (level == LevelList.None) continue;
                
                var levelString = level.ToString();
                levelConditions.Add(levelString, false);
            }
            return levelConditions;
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
        
        #endregion
    }
}