using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Alphabet.Data;
using Alphabet.Stage;

using Random = UnityEngine.Random;

namespace Alphabet.Letter
{
    public class LetterPooler : MonoBehaviour
    {
        #region Fields & Property

        [Header("Pooler")]
        [SerializeField] private LetterController letterPrefabs;
        [SerializeField] private GameObject letterParent;
        [SerializeField] private int defaultPoolCapacity;
        [SerializeField] private int maxPoolSize;

        [Header("Data")]
        private int _stageIndex;
        private List<LetterData> _letterDatas;
        private ObjectPool<LetterController> _letterPool;
        
        public List<Transform> SpawnedLetters { get; private set; }
        public List<LetterData> SpawnedLetterDatas { get; private set; }
                
        [Header("Reference")]
        [SerializeField] private LetterContainer letterContainer;

        #endregion

        #region MonoBehaviour Callbacks
        
        private void Start()
        {
            InitializePooler();
        }

        #endregion

        #region Pooling Methods
        
        // Invoked when creating an item to populate the object pool
        private LetterController CreateLetter()
        {
            var letterObject = Instantiate(letterPrefabs, letterParent.transform, false);
            letterObject.ObjectPool = _letterPool;
            return letterObject;
        }
        
        // Invoked when retrieving the next item from the object pool
        private void OnGetFromPool(LetterController pooledObject)
        {
            pooledObject.gameObject.SetActive(true);
        }
        
        // Invoked when returning an item to the object pool
        private void OnReleaseToPool(LetterController pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }
        
        // Invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
        private void OnDestroyPooledObject(LetterController pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }
        
        #endregion

        #region Methods

        // !-- Initialization
        private void InitializePooler()
        {
            _letterPool = new ObjectPool<LetterController>(CreateLetter, OnGetFromPool, OnReleaseToPool,
                     OnDestroyPooledObject, collectionCheck: true, defaultPoolCapacity, maxPoolSize);
            
            SpawnedLetters = new List<Transform>();
            SpawnedLetterDatas = new List<LetterData>();
        }

        public void InitializePoolData(IReadOnlyList<LetterData> datas)
        {
            _letterDatas = new List<LetterData>();
            _letterDatas.AddRange(datas);
        }
        
        // !-- Core Functionality
        public void CallLetterPool(LetterSpawns[] spawns)
        {
            UpdatePoolData();
            GenerateLetter(spawns);
        }

        // TODO: Panggil method ini dulu waktu akan generate letter
        private void UpdatePoolData()
        {
            SpawnedLetters.Clear();
            SpawnedLetterDatas.Clear();

            _stageIndex = StageManager.Instance.CurrentStageIndex;
        }

        private void GenerateLetter(LetterSpawns[] spawns)
        {
            if (_letterDatas == null)
            {
                Debug.LogError("letter data null!");
                return;
            }

            var latestLetterIndices = new HashSet<int>();
            var latestPointIndices = new HashSet<int>();
            var letterSpawns = spawns[_stageIndex];

            for (var i = 0; i < letterSpawns.AmountOfLetter; i++)
            {
                int randomLetterIndex;
                int randomPointIndex;
                
                do
                {
                    randomLetterIndex = _letterDatas.Count <= letterSpawns.AmountOfLetter 
                            ? i : Random.Range(0, _letterDatas.Count - 1);
                    randomPointIndex = Random.Range(0, letterSpawns.SpawnPointTransforms.Length - 1);
                } while (latestLetterIndices.Contains(randomLetterIndex) || latestPointIndices.Contains(randomPointIndex));

                latestLetterIndices.Add(randomLetterIndex);
                latestPointIndices.Add(randomPointIndex);

                AdjustSpecialCases(ref randomLetterIndex, i);

                var letter = _letterPool.Get();
                var letterData = letterContainer.GetLetterDataById(_letterDatas[randomLetterIndex].LetterId);
                
                letter.InitializeLetterData(letterData, i + 1);
                letter.transform.position = letterSpawns.SpawnPointTransforms[randomPointIndex].position;
                
                SpawnedLetters.Add(letter.transform);
                SpawnedLetterDatas.Add(letterData);
                _letterDatas.RemoveAt(randomLetterIndex);
            }
        }

        private void AdjustSpecialCases(ref int letterIndex, int iteration)
        {
            if (_letterDatas.Count == 3 && iteration == 1)
                letterIndex = 2;
            else if (_letterDatas.Count == 2 && iteration == 2)
                letterIndex = 1;
            else if (_letterDatas.Count == 1 && iteration == 3)
                letterIndex = 0;
        }

        #endregion
    }
}
