using System;
using UnityEngine;
using Alphabet.Stage;

namespace Alphabet.Entities.Player
{
    public class PlayerSpawner : MonoBehaviour
    {
        #region Fileds & Properties

        [Header("Data")]
        [SerializeField] private Transform[] playerSpawnPoints;
        public static event Action OnPlayerSpawned;

        #endregion
        
        #region MonoBehaviour Callbacks

        void OnEnable()
        {
            OnPlayerSpawned += SpawnPlayer;
        }

        private void OnDisable()
        {
            OnPlayerSpawned -= SpawnPlayer;
        }

        private void Start()
        {
            SpawnPlayerEvent();
        }

        #endregion

        #region Methods

        public static void SpawnPlayerEvent() => OnPlayerSpawned?.Invoke();

        private void SpawnPlayer()
        {
            var stageManager = StageManager.Instance;
            if (playerSpawnPoints.Length < stageManager.StageCount)
            {
                Debug.LogWarning("spawn points kurenx brok");
                return;
            }

            var playerObject = GameObject.FindGameObjectWithTag("Player");
            playerObject.transform.position = playerSpawnPoints[stageManager.CurrentStageIndex].position;
        }

        #endregion
    }
}
