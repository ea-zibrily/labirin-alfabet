using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LabirinKata.DesignPattern.Singleton;
using LabirinKata.Enum;

namespace LabirinKata.Database
{
    public class GameDatabase : MonoDDOL<GameDatabase>
    {
        /*
         * TODO: Drop all data yang disimpen disini wak
         * List Data Tersimpan
         * 1. Locked/Unlocked Letter
         * 2. Stage Clear
         */
        
        #region Saved Variable

        //-- Main Database
        private Dictionary<int, bool> _letterConditions;
        private Dictionary<string, bool> _levelConditions ;
        
        //-- Constant Variable
        private const int LETTER_COUNT = 26;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            InitializeData();
        }

        #endregion
        
        #region Labirin Kata Callbacks
        
        //-- Initialization
        private void InitializeData()
        {
            if (_letterConditions != null || _levelConditions != null) return;
            
            _letterConditions = new Dictionary<int, bool>();
            _levelConditions = new Dictionary<string, bool>();

            for (var i = 0; i < LETTER_COUNT; i++)
            {
                var letterKey = i + 1;
                _letterConditions.Add(letterKey, false);
            }
            
            foreach (LevelList level in System.Enum.GetValues(typeof(LevelList)))
            {
                if (level == LevelList.None) continue;
                
                var levelString = level.ToString();
                _levelConditions.Add(levelString, false);
            }
            
            Debug.Log("init letter and level database");
        }
        
        //-- Core Functionality
        public void SaveLetterConditions(int letterId, bool value)
        {
            if (_letterConditions.ContainsKey(letterId))
            {
                _letterConditions[letterId] = value;
                Debug.Log($"save {_letterConditions[letterId]}");
            }
            else
            {
                Debug.LogWarning("gapunya keyny kang");
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
                _levelConditions.Add(levelName, value);
                Debug.Log($"save {_levelConditions[levelName]}");
            }
            else
            {
                Debug.LogWarning("gapunya keyny kang");
            }
        }

        public bool LoadLevelConditions(string levelName)
        {
            return _levelConditions[levelName];
        }
       
        
        #endregion
    }
}