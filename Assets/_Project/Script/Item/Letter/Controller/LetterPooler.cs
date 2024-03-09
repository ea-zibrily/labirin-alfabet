using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Alphabet.Data;
using UnityEngine.Pool;
using Alphabet.Stage;

using Random = UnityEngine.Random;

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

        private int _stageIndex;
        private ObjectPool<LetterController> _letterPool;
        
        public List<LetterData> AvailableLetterDatas { get; private set; }
        public List<Transform> AvailableSpawnPoints { get; private set; }

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

            AvailableLetterDatas = new List<LetterData>();
            AvailableSpawnPoints = new List<Transform>();
        }

        // TODO: Panggil method ini dulu waktu akan generate letter
        private void InitializeGenerator()
        {
            AvailableLetterDatas.Clear();
            AvailableSpawnPoints.Clear();
            _stageIndex = StageManager.Instance.CurrentStageIndex;
        }
        
        // !-- Core Functionality
        public void CallLetterPool(LetterSpawns[] spawns, List<LetterData> datas)
        {
            InitializeGenerator();
            GenerateLetter(spawns, datas);
        }

        private void GenerateLetter(LetterSpawns[] spawns, List<LetterData> datas)
        {
            if (datas == null)
            {
                Debug.LogError("objects null!");
                return;
            }
            
            var letterDatas = new List<LetterData>(datas);
            var letterSpawns = spawns[_stageIndex];
            Debug.Log($"Received {letterDatas.Count} letter data.");

            var latestLetterIndices = new HashSet<int>();
            var latestPointIndices = new HashSet<int>();
            
            for (var i = 0; i < letterSpawns.AmountOfLetter; i++)
            {
                int randomLetterId;
                int randomPointIndex;
                
                do
                {
                    randomLetterId = Random.Range(1, letterDatas.Count);
                    randomPointIndex = Random.Range(0, letterSpawns.SpawnPointTransforms.Length - 1);
                } while (latestLetterIndices.Contains(randomLetterId) || latestPointIndices.Contains(randomPointIndex));
                
                latestLetterIndices.Add(randomLetterId);
                latestPointIndices.Add(randomPointIndex);

                var letter = _letterPool.Get();
                var letterData = letterContainer.GetLetterDataById(randomLetterId);

                letter.InitializeLetterData(letterData, i + 1);
                letter.transform.position = letterSpawns.SpawnPointTransforms[randomPointIndex].position;
                
                AvailableLetterDatas.Add(letterData);
            }
            
            SetAvailableSpawnPoint(latestPointIndices, spawns);      
        }
        
        // !-- Helper/Utilities
        private void SetAvailableSpawnPoint(HashSet<int> value, LetterSpawns[] spawns)
        {
            var removedPointIndex = value.ToList();
            var spawnPoints = spawns[_stageIndex].SpawnPointTransforms;
            
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (removedPointIndex.Contains(i)) continue;
                AvailableSpawnPoints.Add(spawnPoints[i]);
            }
        }

        #endregion
    }
}
