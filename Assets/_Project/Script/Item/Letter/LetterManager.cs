using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LabirinKata.Enum;
using KevinCastejon.MoreAttributes;
using LabirinKata.Stage;
using Random = UnityEngine.Random;

namespace LabirinKata.Entities.Item
{
    public class LetterManager : MonoBehaviour
    {
        #region Struct

        [Serializable]
        private struct LetterAmount
        {
            public StageList StageName;
            public int AmountOfLetter;
        }
        
        #endregion
        
        #region Variable
        
        [Header("Letter")] 
        [SerializeField] private LetterAmount[] letterAmount;
        [SerializeField] private GameObject[] letterPrefabs;
        [SerializeField] private int amountOfItem;
        
        private GameObject[] letterSpawnObject;

        [Header("Save")] 
        [SerializeField] [ReadOnly] private List<GameObject> lockedLetterObject;
        [SerializeField] [ReadOnly] private List<GameObject> unlockedLetterObject;
        
        private const string LETTER_KEY_PREFFIX = "Letter_";
        private const string LATEST_LETTER_KEY_INDEX = "Latest";
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Start()
        {
            InitializeLetterObject();
            InitializeSpawnPosition();
            SpawnLetter(true);
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        //-- Initialization
        public void InitializeLetterObject()
        {
            for (var i = 0; i < letterPrefabs.Length; i++)
            {
                var prefabName = letterPrefabs[i].GetComponent<LetterController>().LetterName;
                var latestLetter = GetLatestLetterCount();
                
                for (var j = 0; j < latestLetter; j++)
                {
                    var letterKey = LETTER_KEY_PREFFIX + j;
                    var letterUnlockName = GetLetterPrefs(letterKey);
                    
                    if (prefabName == letterUnlockName)
                    {
                        Debug.LogWarning($"add {prefabName} in unlocked");
                        unlockedLetterObject.Add(letterPrefabs[i]);
                    }
                    else
                    {
                        lockedLetterObject.Add(letterPrefabs[i]);
                        Debug.LogWarning($"add {prefabName} in locked");
                    }
                }
            }
        }
        
        private void InitializeSpawnPosition()
        {
            letterSpawnObject = GameObject.FindGameObjectsWithTag("SpawnPoint");
            Debug.Log(letterSpawnObject.Length);
        }
        
        //-- Core Functionality
        public void AddUnlockLetter(GameObject value)
        {
            unlockedLetterObject.Add(value);
            
            if (lockedLetterObject.Contains(value))
            {
                Debug.LogWarning("remove value masze");
                lockedLetterObject.Remove(value);
            }
        }
        
        private void SpawnLetter(bool isLock)
        {
            var usedRandomIndices = new HashSet<int>();
            var currentStageIndex = StageManager.Instance.CurrentStageIndex;
            var letterObjects = isLock ? lockedLetterObject : unlockedLetterObject;

            for (var i = 0; i < letterAmount[currentStageIndex].AmountOfLetter; i++)
            {
                int randomLetterIndex;
                int randomPointIndex;
                
                do
                {
                    randomLetterIndex = Random.Range(0, letterObjects.Count - 1);
                    randomPointIndex = Random.Range(0, letterSpawnObject.Length - 1);
                } while (!usedRandomIndices.Add(randomLetterIndex) || !usedRandomIndices.Add(randomPointIndex));

                GameObject letterObject = Instantiate(letterObjects[randomLetterIndex]);
                letterObject.transform.position = letterSpawnObject[randomPointIndex].transform.position;
            }
        }
        
        #endregion

        #region Letter Prefs Callbacks
        
        //-- Core Functionality
        public void SaveUnlockedLetters()
        {
            var latestUnlockIndex = GetLatestLetterCount() is 0 ? GetLatestLetterCount() : GetLatestLetterCount() - 1;
            Debug.Log(latestUnlockIndex);
            
            foreach (var unlockLetter in unlockedLetterObject)
            {
                var letterKey = LETTER_KEY_PREFFIX + latestUnlockIndex;
                SetLetterPrefs(letterKey, unlockLetter.GetComponent<LetterController>().LetterName);
                latestUnlockIndex++;
            }
            
            SetLatestLetterCount(unlockedLetterObject.Count);
        }
        
        private void SetLetterPrefs(string key, string value) => PlayerPrefs.SetString(key, value);
        private string GetLetterPrefs(string key) => PlayerPrefs.GetString(key);
        
        private void SetLatestLetterCount(int value) => PlayerPrefs.SetInt(LATEST_LETTER_KEY_INDEX, value);
        private int GetLatestLetterCount() => PlayerPrefs.GetInt(LATEST_LETTER_KEY_INDEX);
        
        #endregion
    }
}