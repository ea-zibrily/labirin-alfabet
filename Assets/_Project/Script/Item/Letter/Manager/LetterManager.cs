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
        
        public LetterPooler LetterPooler => _letterPooler;
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
        }
        
        private void InitializeLetterDatas()
        {
            // if (LetterContainer.LetterDatas.Length < GameDatabase.LETTER_COUNT)
            // {
            //     Debug.LogError("letter prefabs kurenx breks");
            //     return;
            // }
            
            foreach (var letter in LetterContainer.LetterDatas)
            {
                var letterId = letter.LetterId;
                var isLetterUnlock = GameDatabase.Instance.LoadLetterConditions(letterId);
                
                if (isLetterUnlock)
                {
                    // Debug.LogWarning($"add unlock {letter}");
                    _unlockedLetterDatas.Add(letter);
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
        public Transform GetAvailablePoint()
        {
            var stageIndex =  StageManager.Instance.CurrentStageIndex;
            var spawnedLetters = LetterPooler.SpawnedLetters;
            var spawnPoints = LetterSpawns[stageIndex].SpawnPointTransforms;

            // Init available
            var availablePoint = new List<Transform>();
            foreach (var point in spawnPoints)
            {
                availablePoint.Add(point);
            }

            // Check available
            for (var i = 0; i < spawnedLetters.Count; i++)
            {
                for (var j = 0; j < spawnPoints.Length; j++)
                {
                    if (spawnedLetters[i].position != spawnPoints[j].position) continue;
                    availablePoint.Remove(spawnPoints[j]);
                }
            }
            
            // Randomize available
            var randomPointIndex = UnityEngine.Random.Range(0, availablePoint.Count - 1);
            Transform targetPoint = availablePoint[randomPointIndex];
            return targetPoint;
        }

        private List<LetterData> GetLetterDatas()
        {
            var currentLevel = StageManager.Instance.CurrentStage.ToString();
            var isLevelCleared = GameDatabase.Instance.LoadLevelConditions(currentLevel);
            
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