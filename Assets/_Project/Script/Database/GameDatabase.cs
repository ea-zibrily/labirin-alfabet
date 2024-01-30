using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LabirinKata.DesignPattern.Singleton;

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

        private Dictionary<int, bool> _letterConditions;
        private Dictionary<string, bool> _levelConditions ;
        
        #endregion

        #region Labirin Kata Callbacks

        public void SaveLetterConditions(int letterId, bool value)
        {
            if (_letterConditions ==  null)
            {
                _letterConditions = new Dictionary<int, bool>();
            }
            _letterConditions.Add(letterId, value);
            Debug.Log($"save {_letterConditions[letterId]}");
        }
        
        public bool LoadLetterConditions(int letterId)
        {
            Debug.Log($"load {_letterConditions[letterId]}");
            return _letterConditions[letterId];
        }
        
        public void SaveLevelConditions(string levelName, bool value)
        {
            if (_levelConditions == null)
            {
                _levelConditions = new Dictionary<string, bool>();
            }
            _levelConditions.Add(levelName, value);
            Debug.Log($"save {_levelConditions[levelName]}");
        }

        public bool LoadLevelConditions(string levelName)
        {
            Debug.Log($"load {_levelConditions[levelName]}");
            return _levelConditions[levelName];
        }
       
        
        #endregion
    }
}