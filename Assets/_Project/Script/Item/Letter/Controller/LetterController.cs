using System.Collections;
using UnityEngine;
using KevinCastejon.MoreAttributes;

using Random = UnityEngine.Random;
using LabirinKata.Data;

namespace LabirinKata.Item
{
    public class LetterController : MonoBehaviour, ITakeable
    {
        #region Fields & Properties

        [Header("Data")] 
        [SerializeField] [ReadOnlyOnPlay] private int letterId;
        [SerializeField] [ReadOnlyOnPlay] private int spawnId;
        [SerializeField] private string letterName;
        [SerializeField] private bool hasLetterTaken;
        private LetterData _letterData;

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
        private SpriteRenderer _spriteRenderer;

        public LetterManager LetterManager { get; private set; }
        public LetterUIManager LetterUIManager { get; private set; }
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            var managerObject = GameObject.FindGameObjectWithTag("LetterManager");   
            LetterManager = managerObject.GetComponentInChildren<LetterManager>();
            LetterUIManager = managerObject.GetComponentInChildren<LetterUIManager>();
        }
        
        private void Start()
        {
            Debug.LogWarning("strat");
            InitializeLetter();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        public void InitializeData(LetterData data) => _letterData = data;

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
                LetterManager.TakeLetterEvent(gameObject);
                hasLetterTaken = true;
            }
            LetterManager.AddAvailableSpawnPoint(transform);
            LetterUIManager.TakeLetterEvent(SpawnId);
                        
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Panggil method ini jika player bertabrakan dengan enemy
        /// </summary>
        public void Lost()
        {
            gameObject.SetActive(true);
            LetterUIManager.LostLetterEvent(SpawnId);

            var spawnPoints = LetterManager.AvailableSpawnPoint;
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
            LetterManager.RemoveAvailableSpawnPoint(randomPointIndex);
            Debug.LogWarning($"remove available point index {randomPointIndex}");
        }

        #endregion
    }
}