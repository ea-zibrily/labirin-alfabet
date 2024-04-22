using System;
using System.Collections.Generic;
using UnityEngine;
using Alphabet.Stage;
using Alphabet.Database;
using Alphabet.Data;
using Alphabet.Gameplay.Controller;

namespace Alphabet.Item
{
    public class LetterManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Data")] 
        [SerializeField] private LetterSpawns[] letterSpawns;
        
        public LetterSpawns[] LetterSpawns => letterSpawns;
        public List<Transform> AvailableSpawnPoint { get; private set; }
        
        // Temp Letter Object Data
        private List<LetterData> _lockedLetterDatas;
        private List<LetterData> _unlockedLetterDatas;
        
        // Letter Event
        public event Action<LetterData> OnTakeLetter;

        [Header("Reference")] 
        [SerializeField] private LetterContainer _letterContainer;
        private LetterInterfaceManager _letterUIManager;
        private LetterPooler _letterPooler;
        private TutorialController _tutorialController;
        
        public LetterContainer LetterContainer => _letterContainer;
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _letterUIManager = GetComponent<LetterInterfaceManager>();
            _letterPooler = GameObject.FindGameObjectWithTag("Pooler").GetComponent<LetterPooler>();
            _tutorialController = GameObject.Find("TutorialController").GetComponent<TutorialController>();
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
            InitializePools();
            
            SpawnLetter();
            _tutorialController.CallTutorial();
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
            }
        }

        private void InitializePools()
        {
            var datas = GetLetterDatas();
            _letterPooler.InitializePoolData(datas);
        }
        
        // !-- Core Functionality
        public void SpawnLetter()
        {
            _letterPooler.CallLetterPool(LetterSpawns);
            _letterUIManager.SetLetterInterface(_letterPooler.SpawnedLetterDatas);
            
            AvailableSpawnPoint = _letterPooler.AvailableSpawnPoints;
        }
        
        public void TakeLetterEvent(LetterData letterData) => OnTakeLetter?.Invoke(letterData);
        
        private void TakeLetter(LetterData value)
        {
            _unlockedLetterDatas.Add(value);

            var valueId = value.LetterId;
            foreach (var lockLetter in _lockedLetterDatas)
            {
                var lockLetterId = lockLetter.LetterId;
                
                if (lockLetterId != valueId) continue;
                _lockedLetterDatas.Remove(lockLetter);
                break;
            }
        }
        
        // !-- Helper/Utilities
        public void AddSpawnPoint(Transform value)
        {
            var originPoints = letterSpawns[StageManager.Instance.CurrentStageIndex].SpawnPointTransforms;
            foreach (var point in originPoints)
            {
                if (value.position != point.position) continue;
                AvailableSpawnPoint.Add(point);
                return;
            }
        }

        public void RemoveSpawnPoint(int value)
        {
            AvailableSpawnPoint.RemoveAt(value);
        } 

        private List<LetterData> GetLetterDatas()
        {
            var currentLevel = StageManager.Instance.CurrentLevelList.ToString();
            var isLevelCleared = GameDatabase.Instance.LoadLevelConditions(currentLevel);

            // TODO: Drop code dibawah jika suda dicek
            var dataString = isLevelCleared ? "_unlockedLetterDatas" : "_lockedLetterDatas";
            Debug.Log(dataString);
            
            return isLevelCleared ? _unlockedLetterDatas : _lockedLetterDatas;
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