using System.Collections.Generic;
using System.Linq;
using LabirinKata.Stage;
using UnityEngine;
using MonoUnity = UnityEngine.Object;

namespace LabirinKata.Item.Letter
{
    public class LetterGenerator
    {
        #region Fields & Properties

        private readonly List<GameObject> _letterObjects;
        private readonly LetterSpawns[] _letterSpawns;
        
        private int _letterGenerateCount;
        private int _stageIndex;
        
        public List<GameObject> AvailableLetterObjects { get; private set; }
        public List<Transform> AvailableSpawnPoints { get; private set; }
        
        #endregion

        #region Labirin Kata Callbacks
        
        // !-- Initialization
        public LetterGenerator(LetterSpawns[] spawns, List<GameObject> objects)
        {
            _letterSpawns = spawns;
            _letterObjects = objects;

            AvailableLetterObjects = new List<GameObject>();
            AvailableSpawnPoints = new List<Transform>();
        }
        
        /// <summary>
        /// Panggil method ini terlebih dahulu saat akan melakukan generate letter
        /// </summary>
        public void InitializeGenerator()
        {
            if (AvailableLetterObjects.Count > 0 || AvailableSpawnPoints.Count > 0)
            {
                Debug.Log("clear available brow");
                AvailableLetterObjects.Clear();
                AvailableSpawnPoints.Clear();
            }
            
            _stageIndex = StageManager.Instance.CurrentStageIndex;
            _letterGenerateCount = _letterSpawns[_stageIndex].AmountOfLetter;
        }
        
        // !-- Core Functionality

        /// <summary>
        /// Pastikan sudah memanggil method InitializeGenerator saat akan memanggil method ini
        /// </summary>
        public void GenerateLetter()
        {
            if (_letterObjects == null)
            {
                Debug.LogError("objects null!");
                return;
            }
            
            var latestLetterIndices = new HashSet<int>();
            var latestPointIndices = new HashSet<int>();
            
            for (var i = 0; i < _letterGenerateCount; i++)
            {
                int randomLetterIndex;
                int randomPointIndex;
                
                do
                {
                    randomLetterIndex = Random.Range(0, _letterObjects.Count - 1);
                    randomPointIndex = Random.Range(0, _letterSpawns[_stageIndex].SpawnPointTransforms.Length - 1);
                } while (latestLetterIndices.Contains(randomLetterIndex) || latestPointIndices.Contains(randomPointIndex));
                
                latestLetterIndices.Add(randomLetterIndex);
                latestPointIndices.Add(randomPointIndex);

                Debug.LogWarning($"remove point index {randomPointIndex}");
                
                GameObject letterObject = MonoUnity.Instantiate(_letterObjects[randomLetterIndex], _letterSpawns[_stageIndex].SpawnParentTransform, false);
                letterObject.GetComponent<LetterController>().SpawnId = i + 1;
                letterObject.transform.position = _letterSpawns[_stageIndex].SpawnPointTransforms[randomPointIndex].position;
                
                AvailableLetterObjects.Add(letterObject);
            }

            SetAvailableSpawnPoint(latestPointIndices);            
        }
        
        // !-- Helper/Utilities
        private void SetAvailableSpawnPoint(HashSet<int> value)
        {
            var removedPointIndex = value.ToList();
            var spawnPoints = _letterSpawns[_stageIndex].SpawnPointTransforms;

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (removedPointIndex.Contains(i)) continue;
                AvailableSpawnPoints.Add(spawnPoints[i]);
            }
        }
        
        #endregion
    }
}