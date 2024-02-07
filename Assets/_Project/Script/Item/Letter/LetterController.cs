using System.Collections;
using UnityEngine;
using KevinCastejon.MoreAttributes;

using Random = UnityEngine.Random;

namespace LabirinKata.Item.Letter
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

            Debug.LogWarning($"add spawn point on {transform.position}");
            
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Panggil method ini jika player bertabrakan dengan enemy
        /// </summary>
        public void Lost()
        {
            gameObject.SetActive(true);
            StartCoroutine(LostRoutine());
        }
        
        private IEnumerator LostRoutine()
        {
            _letterUIManager.LostLetterEvent(SpawnId);

            var lerpRatio = 0f;
            var elapsedTime = 0f;

            var spawnPoints = _letterManager.AvailableSpawnPoint;
            var randomPointIndex = Random.Range(0, spawnPoints.Count - 1);
            var randomPoint = spawnPoints[randomPointIndex].position;
            
            while (lerpRatio < moveDelay)
            {
                elapsedTime += Time.deltaTime;
                lerpRatio = elapsedTime / lerpDuration;
                transform.position = Vector3.Lerp(transform.position, randomPoint, lerpRatio);
                yield return null;
            }
            
            transform.position = randomPoint;
            _letterManager.RemoveAvailableSpawnPoint(randomPointIndex);
        }
        
        // !-- Helper/Utilities
        private Vector3 GetAvailablePosition()
        {
            var spawnPoints = _letterManager.AvailableSpawnPoint;
            var randomIndex = Random.Range(0, spawnPoints.Count - 1);
            
            return spawnPoints[randomIndex].position;
        }

        #endregion
    }
}