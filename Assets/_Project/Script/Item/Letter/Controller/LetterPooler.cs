using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Alphabet.Data;
using UnityEngine.Pool;
using Alphabet.Stage;

using Random = UnityEngine.Random;
using Unity.VisualScripting;
using UnityEngine.Profiling;

namespace Alphabet.Item
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

        // public List<Transform> AvailableSpawnPoints { get; private set; }

        [Header("Reference")]
        [SerializeField] private LetterContainer letterContainer;

        #endregion

        #region MonoBehaviour Callbacks
        
        private void Start()
        {
            InitializePooler();
        }

        #endregion

        #region Object Pooling Callbacks
        
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

        #region Labirin Kata Callbacks

        // !-- Initialization
        private void InitializePooler()
        {
            _letterPool = new ObjectPool<LetterController>(CreateLetter, OnGetFromPool, OnReleaseToPool,
                     OnDestroyPooledObject, collectionCheck: true, defaultPoolCapacity, maxPoolSize);
            
            SpawnedLetters = new List<Transform>();
            SpawnedLetterDatas = new List<LetterData>();
            // AvailableSpawnPoints = new List<Transform>();
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
            // AvailableSpawnPoints.Clear();
            
            _stageIndex = StageManager.Instance.CurrentStageIndex;
        }

        private void GenerateLetter(LetterSpawns[] spawns)
        {
            Profiler.BeginSample("Generate Letter");
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
                int randomLetterId;
                int randomPointIndex;
                
                do
                {
                    randomLetterId = Random.Range(1, _letterDatas.Count);
                    randomPointIndex = Random.Range(0, letterSpawns.SpawnPointTransforms.Length - 1);
                } while (latestLetterIndices.Contains(randomLetterId) || latestPointIndices.Contains(randomPointIndex));
                
                latestLetterIndices.Add(randomLetterId);
                latestPointIndices.Add(randomPointIndex);
                
                var letter = _letterPool.Get();
                var letterData = letterContainer.GetLetterDataById(_letterDatas[randomLetterId].LetterId);
                
                letter.InitializeLetterData(letterData, i + 1);
                letter.transform.position = letterSpawns.SpawnPointTransforms[randomPointIndex].position;
                
                SpawnedLetters.Add(letter.transform);
                SpawnedLetterDatas.Add(letterData);
                _letterDatas.RemoveAt(randomLetterId);
            }
            
            // SetAvailableSpawnPoint(latestPointIndices, spawns);
            Profiler.EndSample();
        }
        
        // !-- Helper/Utilities
        private void SetAvailableSpawnPoint(HashSet<int> value, LetterSpawns[] spawns)
        {
            var removedPointIndex = value.ToList();
            var spawnPoints = spawns[_stageIndex].SpawnPointTransforms;
            
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (removedPointIndex.Contains(i)) continue;
                // AvailableSpawnPoints.Add(spawnPoints[i]);
            }
        }

        #endregion
    }
}
