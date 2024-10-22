﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Alphabet.Enum;
using Alphabet.Item;
using Alphabet.Stage;
using Alphabet.Letter;
using Alphabet.Managers;
using Alphabet.Entities.Enemy;
using Alphabet.Gameplay.EventHandler;

using Random = UnityEngine.Random;

namespace Alphabet.Entities.Player
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class PlayerManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Health")] 
        [SerializeField] private int healthCount;
        [SerializeField] private GameObject[] healthUIObjects;

        private int _currentHealthCount;
        private bool _isPlayerDead;
        private const float DIE_DELAY = 0.5f;
        
        public GameObject[] HealthUIFills { get; private set; }
        public int CurrentHealthCount
        {
            get => _currentHealthCount;
            set => _currentHealthCount = value;
        }
        
        [Header("Invulnerability Frame")] 
        [SerializeField] private int flashNumber;
        [SerializeField] private float flashDuration;
        [SerializeField] private Color flashColor;
        
        [Header("Objective")] 
        private LetterObject[] letterObjects;
        private int _currentStageIndex;
        private int _currentLetterAmount;

        [Header("Effect")]
        [SerializeField] private GameObject healEffect;
        [SerializeField] private GameObject speedEffect;
        public Dictionary<BuffType, bool> HasBuffEffect;

        public GameObject HealEffect => healEffect;
        public GameObject SpeedEffect => speedEffect;
        
        [Header("Reference")]
        private PlayerController _playerController;
        private PlayerKnockBack _playerKnockBack;
        private PlayerFlash _playerFlash;

        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            var playerObject = transform.parent.gameObject;
            _playerController = playerObject.GetComponent<PlayerController>();
            _playerKnockBack = playerObject.GetComponent<PlayerKnockBack>();

            var playerSkeleton = _playerController.GetComponentInChildren<SkeletonAnimation>().Skeleton;
            _playerFlash = new PlayerFlash(6, 7, flashColor, flashDuration, flashNumber, playerSkeleton);
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

            SpeedEffect.SetActive(false);
            HealEffect.SetActive(false);
            
            HasBuffEffect = new Dictionary<BuffType, bool>();
            foreach (BuffType level in System.Enum.GetValues(typeof(BuffType)))
            {
                HasBuffEffect.Add(level, false);
            }
        }
        
        #endregion
        
        #region Health Methods
        
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

            _currentHealthCount = healthCount;
            _isPlayerDead = false;
        }
        
        // !-- Core Functionality
        private void DecreaseHealth()
        {
            var healthIndex = _currentHealthCount - 1;
            HealthUIFills[healthIndex].SetActive(false);
            _currentHealthCount--;
            
            if (_currentHealthCount <= 0)
            {
                _currentHealthCount = 0;
                StartCoroutine(PlayerDieRoutine());
            }
        }
        
        private IEnumerator PlayerDieRoutine()
        {
            yield return new WaitForSeconds(DIE_DELAY);
            _isPlayerDead = true;
            GameEventHandler.GameOverEvent(LoseType.Death);
        }
        
        private void KnockedBack(GameObject triggeredObject)
        {
            var playerDirection = _playerController.PlayerInputHandler.Direction;
            var enemyDirection = _playerController.transform.position - triggeredObject.transform.position;
            enemyDirection.Normalize();
            
            _playerKnockBack.CallKnockBack(enemyDirection, Vector2.right, playerDirection);
        }
        
        public void CanceledBuff()
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
        
        #region Objective Methods
        
        // !-- Initialization
        private void InitializeLetterObject()
        {
            var objectSize = StageManager.Instance.StageCount;
            letterObjects = new LetterObject[objectSize];
            
            for (int i = 0; i < letterObjects.Length; i++)
            {
                letterObjects[i].LetterObjects = new List<GameObject>();
            }
        }
        
        // !-- Core Functionality
        private void CollectLetter(GameObject letter)
        {
            SetStageData();
            var letterCollects = letterObjects[_currentStageIndex].LetterObjects;
            letterCollects.Add(letter);
        }
        
        private void LostLetter()
        {
            var letterCollects = letterObjects[_currentStageIndex].LetterObjects;
            if (letterCollects.Count < 1 || letterCollects.Count >= _currentLetterAmount) return;

            var randomLetter = Random.Range(0, letterCollects.Count - 1);
            letterCollects[randomLetter].SetActive(true);
            letterCollects[randomLetter].GetComponent<LetterLost>().Lost();
            letterCollects.RemoveAt(randomLetter);
        }

        public void ResetLetter()
        {
            var letterCollects = letterObjects[_currentStageIndex].LetterObjects;

            if (letterCollects.Count < _currentLetterAmount) return;
            foreach (var collect in letterCollects)
            {
                collect.GetComponent<LetterController>().ReleaseLetter();  
            }
        }
        
        // !-- Helper/Utilities
        private void SetStageData()
        {
            _currentStageIndex = StageManager.Instance.CurrentStageIndex;
            _currentLetterAmount = StageManager.Instance.LetterManager.LetterSpawns[_currentStageIndex].AmountOfLetter;
        }
        
        #endregion

        #region Utilities

        private void TriggeredFeedback(TagFeedback tag, GameObject triggerObject)
        {
            switch (tag)
            {
                case TagFeedback.Enemy:
                    FindObjectOfType<AudioManager>().PlayAudio(Musics.HitSfx);
                    DecreaseHealth();
                    _playerController.StopMovement();
                    
                    CameraEventHandler.CameraShakeEvent();
                    KnockedBack(triggerObject);
                    StartCoroutine(_playerFlash.FlashWithTimeRoutine());
                    
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
            else if (other.CompareTag("Pick"))
            {
                GetComponent<CapsuleCollider2D>().isTrigger = false;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (_isPlayerDead || !_playerController.CanMove) return;
            
            var collider = GetComponent<CapsuleCollider2D>();
            if (collider.isTrigger) return;
            collider.isTrigger = true;
        }

        #endregion
    }
}