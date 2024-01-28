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
            StartCoroutine(LostRoutine());
        }
        
        private IEnumerator LostRoutine()
        {
            gameObject.SetActive(true);
            _letterUIManager.LostLetterEvent(letterId);
            
            var position = transform.position;
            var randomizeX = Random.Range(position.x - 4f, position.x + 4f);
            var randomizeY = Random.Range(position.y - 4f, position.y + 4f);
            var target = new Vector2(randomizeX, randomizeY);
            
            transform.position = Vector2.MoveTowards(position, target, 7f);
            
            yield return new WaitForSeconds(0.5f);
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
                if (collider.CompareTag("Item"))
                {
                    Debug.Log($"triggered with {collider.name}");
                    return true;
                }
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, _boxCollider2D.size);
            Gizmos.color = Color.red;
        }

        #endregion
    }
}