using System.Collections.Generic;
using LabirinKata.Stage;
using UnityEngine;
using MonoUnity = UnityEngine.Object;

namespace LabirinKata.Item.Letter
{
    public class LetterGenerator
    {
        #region Variable

        private readonly List<GameObject> _letterObjects;
        private readonly LetterSpawns[] _letterSpawns;
        
        private int _letterGenerateCount;
        private int _stageIndex;
        private Transform _letterParentTransform;
        
        public List<GameObject> AvailableLetterObjects { get; private set; }
        public List<Transform> AvailableSpawnPoints { get; private set; }
        
        #endregion

        #region Labirin Kata Callbacks
        
        //-- Initialization
        public LetterGenerator(LetterSpawns[] spawns, List<GameObject> objects)
        {
            _letterSpawns = spawns;
            _letterObjects = objects;
        }
        
        //-- Core Functionality
        
        /// <summary>
        /// Panggil method ini terlebih dahulu saat akan melakukan generate letter
        /// </summary>
        public void InitializeGenerator()
        {
            if (AvailableLetterObjects == null)
            {
                AvailableLetterObjects = new List<GameObject>();
            }
            
            if (AvailableSpawnPoints == null)
            {
                AvailableSpawnPoints = new List<Transform>();
            }
            
            AvailableLetterObjects.Clear();
            AvailableSpawnPoints.Clear();
            
            _stageIndex = StageManager.Instance.CurrentStageIndex;
            _letterGenerateCount = _letterSpawns[_stageIndex].AmountOfLetter;
            AddAvailableSpawnPoint();
        }
        
        private void AddAvailableSpawnPoint()
        {
            foreach (var spawnPoint in _letterSpawns[_stageIndex].SpawnPointTransforms)
            {
                AvailableSpawnPoints.Add(spawnPoint);
            }
        }
        
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
                
                GameObject letterObject = MonoUnity.Instantiate(_letterObjects[randomLetterIndex], _letterSpawns[_stageIndex].SpawnParentTransform, false);
                letterObject.GetComponent<LetterController>().SpawnId = i + 1;
                letterObject.transform.position = _letterSpawns[_stageIndex].SpawnPointTransforms[randomPointIndex].position;
                
                AvailableLetterObjects.Add(letterObject);
                AvailableSpawnPoints.RemoveAt(randomPointIndex);
            }
        }
        
        #endregion
    }
}