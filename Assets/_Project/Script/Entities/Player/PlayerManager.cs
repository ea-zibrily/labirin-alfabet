using System;
using System.Collections;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Item;
using LabirinKata.Stage;
using LabirinKata.Item.Letter;
using LabirinKata.Item.Reinforcement;
using LabirinKata.Gameplay.EventHandler;


using Random = UnityEngine.Random;

namespace LabirinKata.Entities.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Health")] 
        [SerializeField] private int healthCount;
        [SerializeField] [ReadOnly] private int currentHealthCount;
        [SerializeField] private GameObject[] healthUIObjects;

        private bool _isPlayerDead;
        
        public GameObject[] HealthUIObjects => healthUIObjects;
        public int CurrentHealthCount
        {
            get => currentHealthCount;
            set => currentHealthCount = value;
        }
        
        [Header("Invulnerability Frame")] 
        [SerializeField] private int flashNumber;
        [SerializeField] private float flashDuration;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color flashColor;
        
        [Header("Objective")] 
        [SerializeField] private LetterObject[] letterObjects;
        
        [Header("Reference")] 
        private GameObject _playerObject;
        private PlayerController _playerController;
        private PlayerKnockBack _playerKnockBack;

        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            // _playerObject = GameObject.FindGameObjectWithTag("Player");
            _playerObject = transform.parent.gameObject;;
            _playerController = _playerObject.GetComponent<PlayerController>();
            _playerKnockBack = _playerObject.GetComponent<PlayerKnockBack>();
        }

        private void OnEnable()
        {
            CameraEventHandler.OnCameraShiftIn += _playerController.StopMovement;
            CameraEventHandler.OnCameraShiftOut += _playerController.StartMovement;
        }

        private void OnDisable()
        {
            CameraEventHandler.OnCameraShiftIn -= _playerController.StopMovement;
            CameraEventHandler.OnCameraShiftOut -= _playerController.StartMovement;
        }

        private void Start()
        {
           InitializeHealth();
           InitializeLetterObject();
        }
        
        #endregion
        
        #region Health Callbacks
        
        // !-- Initialization
        private void InitializeHealth()
        {
            if (healthCount != healthUIObjects.Length)
            {
                Debug.LogWarning("health count ga sama dgn isi health ui lur");
                return;
            }
            
            currentHealthCount = healthCount;
            _isPlayerDead = false;
        }
        
        // !-- Core Functionality
        private void DecreaseHealth()
        {
            var healthIndex = currentHealthCount - 1;
            healthUIObjects[healthIndex].SetActive(false);
            currentHealthCount--;
            
            if (currentHealthCount <= 0)
            {
                currentHealthCount = 0;
                StartCoroutine(PlayerDieRoutine());
            }
        }
        
        private IEnumerator PlayerDieRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            _isPlayerDead = true;
            GameEventHandler.GameOverEvent();
        }
        
        private void KnockedBack(GameObject triggeredObject)
        {
            var playerDirection = _playerController.PlayerInputHandler.Direction;
            var enemyDirection = _playerObject.transform.position - triggeredObject.transform.position;
            enemyDirection.Normalize();
            
            _playerKnockBack.CallKnockBack(enemyDirection, Vector2.right, playerDirection);
        }
        
        private IEnumerator IframeRoutine()
        {
            var tempFlashNum = 0;
            var playerSpriteRenderer = _playerObject.GetComponentInChildren<SpriteRenderer>();
            Physics2D.IgnoreLayerCollision(6, 7, true);
            
            while (tempFlashNum < flashNumber)
            {
                playerSpriteRenderer.color  = flashColor;
                yield return new WaitForSeconds(flashDuration);
                
                playerSpriteRenderer.color = defaultColor;
                yield return new WaitForSeconds(flashDuration);
                tempFlashNum++;
            }
            
            Physics2D.IgnoreLayerCollision(6, 7, false);
        }
        
        private void CanceledBuff()
        {
            var buffObjects = GameObject.FindGameObjectsWithTag("Item");
            foreach (var buff in buffObjects)
            {
                if (!buff.TryGetComponent<BuffItem>(out var buffItem)) continue;
                
                if (buffItem.gameObject.activeSelf && buffItem.IsBuffActive)
                {
                    buffItem.BuffComplete();
                    break;
                }
            }
        }
        
        #endregion

        #region Objective Callbacks
        
        // !-- Initialization
        private void InitializeLetterObject()
        {
            if (letterObjects != null) return;
            
            var objectSize = StageManager.Instance.StageCount;
            letterObjects = new LetterObject[objectSize];
        }
        
        // !-- Core Functionality
        private void CollectLetter(GameObject letter)
        {
            var currentStageIndex = StageManager.Instance.CurrentStageIndex;
            letterObjects[currentStageIndex].LetterObjects.Add(letter);
        }
        
        private void LostLetter()
        {
            var stageIndex = StageManager.Instance.CurrentStageIndex;
            var letterAmount = StageManager.Instance.LetterManager.LetterSpawns[stageIndex].AmountOfLetter;
            var letterCollects = letterObjects[stageIndex].LetterObjects;

            if (letterCollects.Count < 1 || letterCollects.Count >= letterAmount) return;

            var randomLetter = Random.Range(0, letterCollects.Count - 1);
            letterCollects[randomLetter].transform.position = _playerObject.transform.position;
            letterCollects[randomLetter].GetComponent<LetterController>().Lost();
            letterCollects.RemoveAt(randomLetter);
        }
        
        #endregion
        
        #region Collider Callbacks
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isPlayerDead || !_playerController.CanMove) return;
            
            if (other.CompareTag("Enemy"))
            {
                _playerController.StopMovement();
                DecreaseHealth();
                CameraEventHandler.CameraShakeEvent();
                KnockedBack(other.gameObject);
                StartCoroutine(IframeRoutine());
                CanceledBuff();
                LostLetter();
            }
            else if (other.CompareTag("Item"))
            {
                var takeableObject = other.GetComponent<ITakeable>();
                takeableObject.Taken();
                
                if (!(takeableObject as LetterController)) return;
                CollectLetter(other.gameObject);
            }
        }
        
        #endregion
    }
}