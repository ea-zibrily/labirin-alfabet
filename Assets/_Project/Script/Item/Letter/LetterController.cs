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

        [Header("Lost")] 
        [SerializeField] private float minRange;
        [SerializeField] private float maxRange;
        [SerializeField] private float moveDelay;
        [SerializeField] private float lerpDuration;
        
        public int LetterId => letterId;
        public string LetterName => letterName;
        public int SpawnId
        {
            get => spawnId;
            set => spawnId = value;
        }
        
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
            StartCoroutine(LostRoutine());
        }
        
        private IEnumerator LostRoutine()
        {
            _letterUIManager.LostLetterEvent(SpawnId);
            
            var elapsedTime = 0f;
            var lerpRatio = 0f;
            var randomPosition = GetAvailablePosition();
            
            while (lerpRatio < moveDelay)
            {
                elapsedTime += Time.deltaTime;
                lerpRatio = elapsedTime / lerpDuration;
                transform.position = Vector3.Lerp(transform.position, randomPosition, lerpRatio);
                yield return null;
            }
            
            transform.position = randomPosition;
        }

        // !-- Helper/Utilities
        private Vector3 GetAvailablePosition()
        {
            var spawnPoints = _letterManager.AvailableSpawnPoint;
            var randomPosition = Random.Range(0, spawnPoints.Count - 1);
            
            _letterManager.RemoveAvailableSpawnPoint(randomPosition);
            return spawnPoints[randomPosition].position;
        }

        /* Reposition with Delay & Teleport
        private IEnumerator LostRoutine()
        {
            _letterUIManager.LostLetterEvent(SpawnId);
            
            var elapsedTime = 0f;
            var lerpRatio = 0f;
            var randomPosition = transform.position + new Vector3(
                Random.Range(minRange, maxRange),
                Random.Range(minRange, maxRange),
                Random.Range(minRange, maxRange)
            );
            
            while (lerpRatio < moveDelay)
            {
                elapsedTime += Time.deltaTime;
                lerpRatio = elapsedTime / lerpDuration;
                transform.position = Vector3.Lerp(transform.position, randomPosition, lerpRatio);
                yield return null;
            }
            
            transform.position = randomPosition;
            Reposition();
        }
        
        // !-- Helper/Utilities
        private void Reposition()
        {
            var spawnPoints = _letterManager.AvailableSpawnPoint;
            var randomPosition = Random.Range(0, spawnPoints.Count - 1);
            
            transform.position = spawnPoints[randomPosition].position;
            Debug.Log("reposition after hit");
            _letterManager.RemoveAvailableSpawnPoint(randomPosition);
        }
        */

        #endregion
    }
}