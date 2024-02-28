using System;
using System.Collections;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Item;
using LabirinKata.Stage;
using LabirinKata.Gameplay.EventHandler;

using Random = UnityEngine.Random;
using LabirinKata.Entities.Enemy;

namespace LabirinKata.Entities.Player
{
    [AddComponentMenu("Labirin Kata/Entities/Player/Player Manager")]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class PlayerManager : MonoBehaviour
    {
        #region Enum

        public enum TagFeedback
        {
            Enemy,
            Item
        }
        
        #endregion

        #region Fields & Properties
        
        [Header("Health")] 
        [SerializeField] private int healthCount;
        [SerializeField] [ReadOnly] private int currentHealthCount;
        [SerializeField] private GameObject[] healthUIObjects;

        private bool _isPlayerDead;
        private const float DIE_DELAY = 0.5f;
        
        public GameObject[] HealthUIFills { get; private set; }
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
            _playerObject = transform.parent.gameObject;
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
                Debug.LogWarning("health count ga sesuai!");
                return;
            }
            
            HealthUIFills = new GameObject[healthUIObjects.Length];
            for (var i = 0; i < HealthUIFills.Length; i++)
            {
                var healthFill = healthUIObjects[i].transform.GetChild(0).gameObject;
                HealthUIFills[i] = healthFill;
            }

            currentHealthCount = healthCount;
            _isPlayerDead = false;
        }
        
        // !-- Core Functionality
        private void DecreaseHealth()
        {
            var healthIndex = currentHealthCount - 1;
            HealthUIFills[healthIndex].SetActive(false);
            currentHealthCount--;
            
            if (currentHealthCount <= 0)
            {
                currentHealthCount = 0;
                StartCoroutine(PlayerDieRoutine());
            }
        }
        
        private IEnumerator PlayerDieRoutine()
        {
            yield return new WaitForSeconds(DIE_DELAY);
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
                    buffItem.DeactivateBuff();
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
            Debug.LogWarning(letterObjects.Length);
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
            var letterCollects = letterObjects[stageIndex].LetterObjects;
            var letterAmount = StageManager.Instance.LetterManager.LetterSpawns[stageIndex].AmountOfLetter;

            if (letterCollects.Count < 1 || letterCollects.Count >= letterAmount) return;
            
            var randomLetter = Random.Range(0, letterCollects.Count - 1);
            // letterCollects[randomLetter].transform.position = _playerObject.transform.position;
            letterCollects[randomLetter].GetComponent<LetterLost>().Lost();
            letterCollects.RemoveAt(randomLetter);
        }
        
        #endregion

        #region Utilities

        private void TriggeredFeedback(TagFeedback tag, GameObject triggerObject)
        {
            switch (tag)
            {
                case TagFeedback.Enemy:
                    _playerController.StopMovement();
                    DecreaseHealth();
                    CameraEventHandler.CameraShakeEvent();
                    KnockedBack(triggerObject);
                    StartCoroutine(IframeRoutine());

                    CanceledBuff();
                    LostLetter();
                    break;
                case TagFeedback.Item:
                    var takeableObject = triggerObject.GetComponent<ITakeable>();
                    takeableObject.Taken();
                    
                    if (!(takeableObject as LetterController)) return;
                    CollectLetter(triggerObject);
                    break;
            }
        }

        #endregion
        
        #region Collider Callbacks
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isPlayerDead || !_playerController.CanMove) return;
            
            if (other.CompareTag("Enemy"))
            {
               if (!other.TryGetComponent(out EnemyBase enemy) || !enemy.CanMove) return;
               
               TriggeredFeedback(TagFeedback.Enemy, other.gameObject);
            }
            else if (other.CompareTag("Item"))
            {
                TriggeredFeedback(TagFeedback.Item, other.gameObject);
            }
        }
        
        #endregion
    }
}