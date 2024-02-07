using System;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Database;
using LabirinKata.Stage;

namespace LabirinKata.Item.Letter
{
    public class LetterManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Letter")] 
        [SerializeField] private GameObject[] letterPrefabs;
        [SerializeField] private LetterSpawns[] letterSpawns;
        [SerializeField] [ReadOnly] private int currentAmountOfLetter;
        
        public LetterSpawns[] LetterSpawns => letterSpawns;
        [field: SerializeField] public List<Transform> AvailableSpawnPoint { get; private set; }
        
        //-- Temp Letter Object Data
        private List<GameObject> _lockedLetterObject;
        private List<GameObject> _unlockedLetterObject;
        
        //-- Letter Event
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
        
        // !-- Initialization
        private void InitializeLetterData()
        {
            _lockedLetterObject = new List<GameObject>();
            _unlockedLetterObject = new List<GameObject>();
            AvailableSpawnPoint = new List<Transform>();
        }
        
        private void InitializeLetterGenerator()
        {
            var currentLevel = StageManager.Instance.CurrentLevelList.ToString();
            var isLevelCleared = GameDatabase.Instance.LoadLevelConditions(currentLevel);

            _letterGenerator = isLevelCleared ? 
                new LetterGenerator(letterSpawns, _unlockedLetterObject) :
                new LetterGenerator(letterSpawns, _lockedLetterObject);
        }
        
        private void InitializeLetterObject()
        {
            if (letterPrefabs.Length < GameDatabase.LETTER_COUNT)
            {
                Debug.LogError("letter prefabs kurenx breks");
                return;
            }
            
            foreach (var letter in letterPrefabs)
            {
                var letterId = letter.GetComponent<LetterController>().LetterId;
                var isLetterUnlock = GameDatabase.Instance.LoadLetterConditions(letterId);
                
                if (isLetterUnlock)
                {
                    _unlockedLetterObject.Add(letter);
                    Debug.LogWarning($"add unlock {letter}");
                    continue;
                }
                _lockedLetterObject.Add(letter);
                Debug.LogWarning($"add lock {letter}");
            }
        }
        
        // !-- Core Functionality
        public void SpawnLetter()
        {
            _letterGenerator.InitializeGenerator();
            _letterGenerator.GenerateLetter();
            _letterUIManager.InitializeLetterInterface(_letterGenerator.AvailableLetterObjects);
            
            if (AvailableSpawnPoint.Count > 0)
            {
                AvailableSpawnPoint.Clear();
            }
            AvailableSpawnPoint = _letterGenerator.AvailableSpawnPoints;
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
        
        // !-- Helper/Utilities
        public void AddAvailableSpawnPoint(Transform value) =>  AvailableSpawnPoint.Add(value);

        public void RemoveAvailableSpawnPoint(int value) => AvailableSpawnPoint.RemoveAt(value);
        
        #endregion
        
        #region Save Letter Callbacks
        
        // !-- Core Functionality
        public void SaveUnlockedLetters()
        {
            if (_unlockedLetterObject == null) return;
            
            foreach (var unlockLetter in _unlockedLetterObject)
            {
                var unlockLetterId = unlockLetter.GetComponent<LetterController>().LetterId;
                GameDatabase.Instance.SaveLetterConditions(unlockLetterId, true);
            }
        }
        
        #endregion
    }
}