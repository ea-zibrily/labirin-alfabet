using System.Collections;
using UnityEngine;
using KevinCastejon.MoreAttributes;

using Random = UnityEngine.Random;

namespace LabirinKata.Item
{
    public class LetterController : MonoBehaviour, ITakeable
    {
        #region Fields & Properties

        [Header("Settings")] 
        [SerializeField] [ReadOnlyOnPlay] private int letterId;
        [SerializeField] [ReadOnlyOnPlay] private int spawnId;
        [SerializeField] private string letterName;
        [SerializeField] private bool hasLetterTaken;

        public int LetterId => letterId;
        public string LetterName => letterName;
        public int SpawnId
        {
            get => spawnId;
            set => spawnId = value;
        }
         
        [Header("Lost")] 
        [SerializeField] private float minRange;
        [SerializeField] private float maxRange;
        [SerializeField] private float moveDelay;
        [SerializeField] private float lerpDuration;


        [Header("Reference")] 
        private LetterManager _letterManager;
        private LetterUIManager _letterUIManager;
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            var letterManagementObject = GameObject.FindGameObjectWithTag("LetterManager");
            
            _letterManager = letterManagementObject.GetComponentInChildren<LetterManager>();
            _letterUIManager = letterManagementObject.GetComponentInChildren<LetterUIManager>();
        }
        
        private void Start()
        {
            InitializeLetter();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        private void InitializeLetter()
        {
            gameObject.name = letterName;
            hasLetterTaken = false;
        }
        
        // !-- Core Functionality
        public void Taken()
        {
            if (!hasLetterTaken)
            {
                _letterManager.TakeLetterEvent(gameObject);
                hasLetterTaken = true;
            }
            _letterManager.AddAvailableSpawnPoint(transform);
            _letterUIManager.TakeLetterEvent(SpawnId);
                        
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Panggil method ini jika player bertabrakan dengan enemy
        /// </summary>
        public void Lost()
        {
            gameObject.SetActive(true);
            _letterUIManager.LostLetterEvent(SpawnId);

            var spawnPoints = _letterManager.AvailableSpawnPoint;
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
            _letterManager.RemoveAvailableSpawnPoint(randomPointIndex);
            Debug.LogWarning($"remove available point index {randomPointIndex}");
        }

        #endregion
    }
}