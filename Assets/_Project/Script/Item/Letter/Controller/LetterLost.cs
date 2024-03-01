using System.Collections;
using System.Collections.Generic;
using Alphabet.Entities.Player;
using UnityEngine;

namespace Alphabet.Item
{
    public class LetterLost : MonoBehaviour
    {
        [Header("Lost")] 
        [SerializeField] private float minRange;
        [SerializeField] private float maxRange;
        [SerializeField] private float moveDelay;
        [SerializeField] private float lerpDuration;

        [Header("Reference")]
        private LetterController _letterController;
        private GameObject _playerObject;

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _letterController = GetComponent<LetterController>();
            _playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        #endregion

        #region Labirin Kata Callbacks

        /// <summary>
        /// Panggil method ini jika player bertabrakan dengan enemy
        /// </summary>
        public void Lost()
        {
            _letterController.ObjectPool.Get();
            transform.position = _playerObject.transform.position;
            _letterController.LetterInterfaceManager.LostLetterEvent(_letterController.SpawnId);
            Debug.LogWarning("re-pool letter");

            var spawnPoints = _letterController.LetterManager.AvailableSpawnPoint;
            var randomPointIndex = Random.Range(0, spawnPoints.Count - 1);
            var randomPoint = spawnPoints[randomPointIndex].position;

            StartCoroutine(LerpToRandomPointRoutine(randomPoint, randomPointIndex));
        }

        // TODO: Coba pake moveDelay = 1, lerpDur= 0.5f
        private IEnumerator LerpToRandomPointRoutine(Vector3 randomPoint, int randomPointIndex)
        {
            var elapsedTime = 0f;

            while (elapsedTime < moveDelay)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
                var lerpRatio = elapsedTime / lerpDuration;
                transform.position = Vector3.Lerp(transform.position, randomPoint, lerpRatio);
            }

            transform.position = randomPoint;
            _letterController.LetterManager.RemoveSpawnPoint(randomPointIndex);
            Debug.LogWarning($"remove available point index {randomPointIndex}");
        }

        #endregion
    }
}
