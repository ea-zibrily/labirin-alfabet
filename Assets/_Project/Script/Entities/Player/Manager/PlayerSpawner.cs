using System.Collections;
using System.Collections.Generic;
using Alphabet.Stage;
using UnityEngine;

namespace Alphabet.Entities.Player
{
    public class PlayerSpawner : MonoBehaviour
    {
        #region Fileds & Properties

        [Header("Data")]
        [SerializeField] private Transform[] playerSpawnPoints;

        #endregion
        
        #region Methods

        public void SpawnPlayer()
        {
            var stageManager = StageManager.Instance;
            if (playerSpawnPoints.Length < stageManager.StageCount)
            {
                Debug.LogWarning("spawn points kurenx brok");
                return;
            }

            var playerObject = transform.parent.gameObject;
            playerObject.transform.position = playerSpawnPoints[stageManager.CurrentStageIndex].position;
            Debug.Log($"Player Spawned at {playerSpawnPoints[stageManager.CurrentStageIndex].position}");
        }

        #endregion
    }
}
