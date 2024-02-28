using System;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Stage;
using LabirinKata.Database;
using LabirinKata.Data;
using LabirinKata.Enum;

namespace LabirinKata.Item
{
    public class LetterManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Spawn Data")] 
        [SerializeField] private LetterSpawns[] letterSpawns;
        [SerializeField] [ReadOnly] private int currentAmountOfLetter;
        
        public LetterSpawns[] LetterSpawns => letterSpawns;
        public List<Transform> AvailableSpawnPoint { get; private set; }
        
        //-- Temp Letter Object Data
        private List<LetterData> _lockedLetterDatas;
        private List<LetterData> _unlockedLetterDatas;
        
        //-- Letter Event
        public event Action<LetterData> OnTakeLetter;

        [Header("Reference")] 
        private LetterInterfaceManager _letterUIManager;
        private LetterPooler _letterPooler;
        [SerializeField] private LetterContainer _letterContainer;
        public LetterContainer LetterContainer => _letterContainer;
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _letterPooler = GameObject.FindGameObjectWithTag("Pooler").GetComponent<LetterPooler>();
            _letterUIManager = GetComponent<LetterInterfaceManager>();
        }

        private void OnEnable()
        {
            OnTakeLetter += TakeLetter;
        }
        
        private void OnDisable()
        {
            OnTakeLetter -= TakeLetter;
        }
        
        private void Start()
        {
            InitializeLetterData();
            InitializeLetterDatas();
            InitializeLetterPooler();
            
            SpawnLetter();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        private void InitializeLetterData()
        {
            _lockedLetterDatas = new List<LetterData>();
            _unlockedLetterDatas = new List<LetterData>();

            AvailableSpawnPoint = new List<Transform>();
        }
        
        private void InitializeLetterPooler()
        {
            var currentLevel = StageManager.Instance.CurrentLevelList.ToString();
            var isLevelCleared = GameDatabase.Instance.LoadLevelConditions(currentLevel);

            _letterPooler.InitializeSpawnDatas(LetterSpawns, isLevelCleared ? _unlockedLetterDatas : _lockedLetterDatas);
        }
        
        private void InitializeLetterDatas()
        {
            if (LetterContainer.LetterDatas.Length < GameDatabase.LETTER_COUNT)
            {
                Debug.LogError("letter prefabs kurenx breks");
                return;
            }
            
            foreach (var letter in LetterContainer.LetterDatas)
            {
                var letterId = letter.LetterId;
                var isLetterUnlock = GameDatabase.Instance.LoadLetterConditions(letterId);
                
                if (isLetterUnlock)
                {
                    _unlockedLetterDatas.Add(letter);
                    Debug.LogWarning($"add unlock {letter}");
                    continue;
                }
                _lockedLetterDatas.Add(letter);
                Debug.LogWarning($"add lock {letter}");
            }
        }
        
        // !-- Core Functionality
        public void SpawnLetter()
        {
            _letterPooler.InitializeGenerator();
            _letterPooler.GenerateLetter();
            _letterUIManager.SetLetterInterface(_letterPooler.AvailableLetterDatas);
            
            AvailableSpawnPoint = _letterPooler.AvailableSpawnPoints;
            currentAmountOfLetter = letterSpawns[StageManager.Instance.CurrentStageIndex].AmountOfLetter;
        }
        
        public void TakeLetterEvent(LetterData letterData) => OnTakeLetter?.Invoke(letterData);
        
        private void TakeLetter(LetterData value)
        {
            _unlockedLetterDatas.Add(value);

            var valueName = value.LetterId;
            foreach (var lockLetter in _lockedLetterDatas)
            {
                var lockLetterName = lockLetter.LetterId;
                
                if (lockLetterName != valueName) continue;
                _lockedLetterDatas.Remove(lockLetter);
                break;
            }
        }
        
        // !-- Helper/Utilities
        public void AddAvailableSpawnPoint(Transform value)
        {
            var originPoints = letterSpawns[StageManager.Instance.CurrentStageIndex].SpawnPointTransforms;
            foreach (var point in originPoints)
            {
                if (value.position != point.position) continue;
                AvailableSpawnPoint.Add(point);
                return;
            }
        }

        public void RemoveAvailableSpawnPoint(int value)
        {
            AvailableSpawnPoint.RemoveAt(value);
        } 
        
        #endregion
        
        #region Save Letter Callbacks
        
        // !-- Core Functionality
        public void SaveUnlockedLetters()
        {
            if (_unlockedLetterDatas == null) return;
            
            foreach (var unlockLetter in _unlockedLetterDatas)
            {
                var unlockLetterId = unlockLetter.LetterId;
                GameDatabase.Instance.SaveLetterCollected(unlockLetterId, true);
            }
        }
        
        #endregion
    }
}