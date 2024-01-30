using System;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Stage;

namespace LabirinKata.Item.Letter
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
        
        public LetterSpawns[] LetterSpawns => letterSpawns;
        public List<Transform> AvailableSpawnPoint { get; private set; }
        
        //-- Temp Letter Object Data
        [SerializeField] [ReadOnly] private List<GameObject> _lockedLetterObject;
        [SerializeField] [ReadOnly] private List<GameObject> _unlockedLetterObject;
        
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
            InitializeLetterGenerator();
            
            SpawnLetter();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        //-- Initialization
        private void InitializeLetterData()
        {
            _lockedLetterObject = new List<GameObject>();
            _unlockedLetterObject = new List<GameObject>();
        }
        
        private void InitializeLetterGenerator()
        {
            // TODO: Ubah parameter generate letter sesuai kondisi level (selesai atau tidak)
            _letterGenerator = new LetterGenerator(letterSpawns, _lockedLetterObject);
        }
        
        private void InitializeLetterObject()
        {
            //-- TODO: Ubah prefs jadi class ddol untuk simpen temporary unlock key
            
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
            _letterGenerator.InitializeGenerator();
            _letterGenerator.GenerateLetter();
            _letterUIManager.InitializeLetterUI(_letterGenerator.SpawnedLetterIndex);
            
            AvailableSpawnPoint.Clear();
            AvailableSpawnPoint = _letterGenerator.AvailableSpawnPoint;
            currentAmountOfLetter = letterSpawns[StageManager.Instance.CurrentStageIndex].AmountOfLetter;
        }
        
        public void TakeLetterEvent(GameObject letterObject) => OnTakeLetter?.Invoke(letterObject);
        
        private void TakeLetter(GameObject value)
        {
            _unlockedLetterObject.Add(value);

            var valueName = value.GetComponent<LetterController>().LetterName;
            foreach (var lockLetter in _lockedLetterObject)
            {
                var lockLetterName = lockLetter.GetComponent<LetterController>().LetterName;
                
                if (lockLetterName != valueName) continue;
                _lockedLetterObject.Remove(lockLetter);
                break;
            }
        }
        
        //-- Helper/Utilities
        public void AddAvailableSpawnPoint(Transform value)
        {
            AvailableSpawnPoint.Add(value);
        }
        
        public void RemoveAvailableSpawnPoint(int value)
        {
            AvailableSpawnPoint.RemoveAt(value);
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