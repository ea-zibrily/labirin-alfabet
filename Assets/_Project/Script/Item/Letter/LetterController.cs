using System.Collections;
using UnityEngine;
using LabirinKata.Stage;

using Random = UnityEngine.Random;

namespace LabirinKata.Item.Letter
{
    public class LetterController : MonoBehaviour, ITakeable
    {
        #region Variable

        [Header("Settings")] 
        [SerializeField] private int letterId;
        [SerializeField] private string letterName;
        [SerializeField] private bool hasLetterTaken;

        [Header("Lost")] 
        [SerializeField] private float minRange;
        [SerializeField] private float maxRange;
        [SerializeField] private float moveDelay;
        [SerializeField] private float lerpDuration;
        
        public string LetterName => letterName;
        public int LetterId
        {
            get => letterId;
            set => letterId = value;
        }

        [Header("Reference")] 
        private BoxCollider2D _boxCollider2D;
        private LetterManager _letterManager;
        private LetterUIManager _letterUIManager;
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
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
        
        //-- Initialization
        private void InitializeLetter()
        {
            gameObject.name = letterName;
            hasLetterTaken = false;
            Debug.Log("init letter obj");
        }
        
        //-- Core Functionality
        public void Taken()
        {
            if (!hasLetterTaken)
            {
                _letterManager.TakeLetterEvent(gameObject);
                hasLetterTaken = true;
            }
            _letterUIManager.TakeLetterEvent(LetterId);
            
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
            _letterUIManager.LostLetterEvent(letterId);
            
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
            RandomizePosition();
        }
        
        private void RandomizePosition()
        {
            var stageIndex = StageManager.Instance.CurrentStageIndex;
            var spawnPoints = _letterManager.LetterSpawns[stageIndex].SpawnPointTransforms;
            
            do
            {
                var randomizePosition = Random.Range(0, spawnPoints.Length - 1);
                transform.position = spawnPoints[randomizePosition].position;
            } while (CheckTriggeredCollider());
        }
        
        //-- Helper/Utilities
        private bool CheckTriggeredCollider()
        {
            var boxSize = _boxCollider2D.size;
            var colliderArea = Physics2D.OverlapBoxAll(transform.position, boxSize, 0);
            
            foreach (var collider in colliderArea)
            {
                if (!collider.CompareTag("Item")) continue;
                Debug.Log($"triggered with {collider.name}");
                return true;
            }

            return false;
        }

        #endregion
    }
}