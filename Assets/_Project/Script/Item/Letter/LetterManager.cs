using System;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Stage;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace LabirinKata.Entities.Item
{
    public class LetterManager : MonoBehaviour
    {
        #region Constant Variable

        private const string LETTER_KEY_PREFFIX = "Letter_";
        private const string LATEST_LETTER_KEY_INDEX = "Latest";

        #endregion
        
        #region Variable
        
        [Header("Letter")] 
        [SerializeField] private GameObject[] letterPrefabs;
        [SerializeField] private LetterSpawns[] letterSpawns;
        [SerializeField] [ReadOnly] private int currentAmountOfLetter;
        
        //-- Temp Letter Object Data
        private List<GameObject> _lockedLetterObject;
        private List<GameObject> _unlockedLetterObject;
        private List<int> _spawnedLetterIndex;

        //-- Event
        public event Action<GameObject> OnTakeLetter;

        [Header("Reference")] 
        private LetterUIManager _letterUIManager;
        
        #endregion
        
        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            OnTakeLetter += TakeLetter;
        }
        
        private void OnDisable()
        {
            OnTakeLetter += TakeLetter;
        }
        
        private void Start()
        {
            _spawnedLetterIndex = new List<int>();
            
            InitializeLetterObject();
            SpawnLetter();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        //-- Initialization
        private void InitializeLetterObject()
        {
            for (var i = 0; i < letterPrefabs.Length; i++)
            {
                var prefabName = letterPrefabs[i].GetComponent<LetterController>().LetterName;
                var latestLetter = GetLatestLetterCount();
                
                if (latestLetter > 0)
                {
                    for (var j = 0; j < latestLetter; j++)
                    {
                        var letterKey = LETTER_KEY_PREFFIX + j;
                        var letterUnlockName = GetLetterSaveName(letterKey);
                        
                        if (prefabName == letterUnlockName)
                        {
                            _unlockedLetterObject.Add(letterPrefabs[i]);
                            continue;
                        }
                        _lockedLetterObject.Add(letterPrefabs[i]);
                    }
                }
                else
                {
                    _lockedLetterObject.Add(letterPrefabs[i]);
                }
            }
        }
        
        //-- Core Functionality
        public void TakeLetterEvent(GameObject letterObject) => OnTakeLetter?.Invoke(letterObject);
        
        private void TakeLetter(GameObject value)
        {
            _unlockedLetterObject.Add(value);
            
            if (_lockedLetterObject.Contains(value))
            {
                Debug.LogWarning("remove value masze");
                _lockedLetterObject.Remove(value);
            }
        }
        
        #endregion

        #region Letter Spawner Callbacks
        
        //-- Initialization
        private void InitializeSpawner()
        {
            currentAmountOfLetter = letterSpawns[StageManager.Instance.CurrentStageIndex].AmountOfLetter;
            _spawnedLetterIndex.Clear();
        }
        
        //-- Core Functionality
        public void SpawnLetter()
        {
            // TODO: Ubah parameter generate letter sesuai kondisi level (selesai atau tidak)
            
            InitializeSpawner();
            GenerateLetter(true);
            _letterUIManager.InitializeLetterUI(_spawnedLetterIndex);
            Debug.Log("spawn bro gasken");
        }
        
        private void GenerateLetter(bool isLetterLock)
        {
            var latestLetterIndices = new HashSet<int>();
            var latestPointIndices = new HashSet<int>();
            
            var currentStageIndex = StageManager.Instance.CurrentStageIndex;
            var tempLetterObjects = isLetterLock ? _lockedLetterObject : _unlockedLetterObject;
            
            for (var i = 0; i < currentAmountOfLetter; i++)
            {
                int randomLetterIndex;
                int randomPointIndex;
                
                do
                {
                    randomLetterIndex = Random.Range(0, tempLetterObjects.Count - 1);
                    randomPointIndex = Random.Range(0, letterSpawns[currentStageIndex].SpawnPointTransforms.Length - 1);
                } while (latestLetterIndices.Contains(randomLetterIndex) || latestPointIndices.Contains(randomPointIndex));
                
                latestLetterIndices.Add(randomLetterIndex);
                latestPointIndices.Add(randomPointIndex);
                
                _spawnedLetterIndex.Add(randomLetterIndex);
                
                GameObject letterObject = Instantiate(tempLetterObjects[randomLetterIndex], letterSpawns[currentStageIndex].SpawnParentTransform, false);
                letterObject.GetComponent<LetterController>().LetterId = i;
                letterObject.transform.position = letterSpawns[currentStageIndex].SpawnPointTransforms[randomPointIndex].transform.position;
                
                Debug.LogWarning($"instantiate -{i}: {letterObject.name}");
            }
        }

        #endregion
        
        #region Save Letter Callbacks
        
        //-- Core Functionality
        public void SaveUnlockedLetters()
        {
            var latestUnlockIndex = GetLatestLetterCount() is 0 ? GetLatestLetterCount() : GetLatestLetterCount() - 1;
            Debug.Log(latestUnlockIndex);
            
            if (_unlockedLetterObject == null) return;
            foreach (var unlockLetter in _unlockedLetterObject)
            {
                var letterKey = LETTER_KEY_PREFFIX + latestUnlockIndex;
                SetLetterSaveName(letterKey, unlockLetter.GetComponent<LetterController>().LetterName);
                latestUnlockIndex++;
            }
            
            SetLatestLetterCount(_unlockedLetterObject.Count);
        }
        
        //-- Helper/Utilities
        private void SetLetterSaveName(string key, string value) => PlayerPrefs.SetString(key, value);
        private string GetLetterSaveName(string key) => PlayerPrefs.GetString(key);
        
        private void SetLatestLetterCount(int value) => PlayerPrefs.SetInt(LATEST_LETTER_KEY_INDEX, value);
        private int GetLatestLetterCount() => PlayerPrefs.GetInt(LATEST_LETTER_KEY_INDEX);
        
        #endregion
    }
}