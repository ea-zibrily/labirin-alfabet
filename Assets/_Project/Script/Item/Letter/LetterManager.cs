using System;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Stage;

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

        //-- Event
        public event Action<GameObject> OnTakeLetter;

        [Header("Reference")] 
        private LetterUIManager _letterUIManager;
        private LetterGenerator _letterGenerator;
        
        #endregion
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            var letterManagerObject = GameObject.FindGameObjectWithTag("LetterManager");
            _letterUIManager = letterManagerObject.GetComponentInChildren<LetterUIManager>();
        }

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
            InitializeLetterData();
            InitializeLetterObject();
            SpawnLetter();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        //-- Initialization
        private void InitializeLetterData()
        {
            _letterGenerator = new LetterGenerator(letterSpawns);
            
            _lockedLetterObject = new List<GameObject>();
            _unlockedLetterObject = new List<GameObject>();
        }
        
        private void InitializeLetterObject()
        {
            foreach (var letter in letterPrefabs)
            {
                var prefabName = letter.GetComponent<LetterController>().LetterName;
                var latestLetter = GetLatestLetterCount();
                
                if (latestLetter > 0)
                {
                    for (var j = 0; j < latestLetter; j++)
                    {
                        var letterKey = LETTER_KEY_PREFFIX + j;
                        var letterUnlockName = GetLetterSaveName(letterKey);
                        
                        if (prefabName == letterUnlockName)
                        {
                            _unlockedLetterObject.Add(letter);
                            Debug.LogWarning($"add unlock {letter}");
                            continue;
                        }
                        
                        _lockedLetterObject.Add(letter);
                        Debug.LogWarning($"add lock {letter}");
                    }
                }
                else
                {
                    _lockedLetterObject.Add(letter);
                }
            }
        }
        
        //-- Core Functionality
        public void SpawnLetter()
        {
            // TODO: Ubah parameter generate letter sesuai kondisi level (selesai atau tidak)
            // TODO: Selesai = unlocked, Tidak = locked
            
            _letterGenerator.InitializeGenerator(_lockedLetterObject);
            _letterGenerator.GenerateLetter();
            _letterUIManager.InitializeLetterUI(_letterGenerator.SpawnedLetterIndex);

            currentAmountOfLetter = letterSpawns[StageManager.Instance.CurrentStageIndex].AmountOfLetter;
        }
        
        public void TakeLetterEvent(GameObject letterObject) => OnTakeLetter?.Invoke(letterObject);
        
        private void TakeLetter(GameObject value)
        {
            Debug.LogWarning($"add unlock {value}");
            _unlockedLetterObject.Add(value);
            
            if (_lockedLetterObject.Contains(value))
            {
                Debug.LogWarning($"remove lock {value}");
                _lockedLetterObject.Remove(value);
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