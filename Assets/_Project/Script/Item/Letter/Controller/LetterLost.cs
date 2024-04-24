using System.Collections;
using System.Collections.Generic;
using Alphabet.Stage;
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
            transform.position = _playerObject.transform.position;
            _letterController.LetterInterfaceManager.LostLetterEvent(_letterController.SpawnId);

            var targetPoint = _letterController.LetterManager.GetAvailablePoint();
            var targetVector = new Vector3(targetPoint.position.x, targetPoint.position.y, targetPoint.position.z);
            StartCoroutine(LerpToRandomPointRoutine(targetVector));
        }

        private Transform GetAvailablePoint()
        {
            var stageIndex =  StageManager.Instance.CurrentStageIndex;
            var spawnedLetters = _letterController.LetterManager.LetterPooler.SpawnedLetters;
            var spawnPoints = _letterController.LetterManager.LetterSpawns[stageIndex].SpawnPointTransforms;

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
            var randomPointIndex = Random.Range(0, availablePoint.Count - 1);
            var targetPoint = availablePoint[randomPointIndex];
            return targetPoint;
        }

        private IEnumerator LerpToRandomPointRoutine(Vector3 randomPoint)
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
        }

        #endregion
    }
}
