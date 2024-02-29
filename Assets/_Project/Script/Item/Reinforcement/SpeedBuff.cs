using System;
using System.Collections;
using LabirinKata.Entities.Player;
using UnityEngine;

namespace LabirinKata.Item
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SpeedBuff : BuffItem, ITakeable
    {
        #region Fields & Properties

        [Header("Speed Buff")]
        [Range(0f, 2.5f)]
        [SerializeField] private float speedUpMultiplier;
        [SerializeField] private float timeDuration;
        [Range(0.5f, 1.5f)]
        [SerializeField] private float timeMultiplier;
        
        [SerializeField] private float _upgradeMoveSpeed;
        [SerializeField] private float _defaultMoveSpeed;
        private float _currentTime;
        
        private bool _isTimerStart;
        private bool _isSpeedUpComplete;
        
        [Header("Buff Effect")]
        [SerializeField] private float flashDuration;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color flashColor;
        
        [Header("Reference")]
        private SpriteRenderer _playerSpriteRenderer;
        private PlayerPickThrow _playerPickThrow;

        #endregion
        
        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            _playerPickThrow.OnPickOrThrow += SetSpeedLimitValues;
        }

        private void OnDisable()
        {
            _playerPickThrow.OnPickOrThrow -= SetSpeedLimitValues;
        }

        private void Update()
        {
            if (!IsBuffActive) return;
            
            if (!_isSpeedUpComplete)
            {
                SpeedUp();

                if (!_isTimerStart) return;
                
                _currentTime += Time.deltaTime;
                if (_currentTime >= timeDuration)
                {
                    _currentTime = 0;
                    _isTimerStart = false;
                    _isSpeedUpComplete = true;
                }
            }
            else
            {
                SlowDown();
            }
        }

        #endregion

        #region Labirin Kata Callbacks

        // !-- Initialization
        protected override void InitializeOnAwake()
        {
            base.InitializeOnAwake();
            _playerPickThrow = PlayerController.GetComponent<PlayerPickThrow>();
            _playerSpriteRenderer = PlayerController.GetComponentInChildren<SpriteRenderer>();
        }

        protected override void InitializeOnStart()
        {
            base.InitializeOnStart();
            _currentTime = 0;
            _isTimerStart = false;
            _isSpeedUpComplete = false;
        }
        
        private void InitializeSpeed()
        {
            _defaultMoveSpeed = PlayerController.CurrentMoveSpeed;
            _upgradeMoveSpeed = _defaultMoveSpeed * speedUpMultiplier;
        }
        
        // !-- Core Functionality
        public void Taken()
        {
            ActivateBuff();
        }

        protected override void ActivateBuff()
        {
            InitializeSpeed();
            base.ActivateBuff();
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            StartSpeedEffect();
        }

        public override void DeactivateBuff()
        {
            base.DeactivateBuff();
            StopSpeedEffect();
            PlayerController.CurrentMoveSpeed = _defaultMoveSpeed;
            GetComponentInChildren<SpriteRenderer>().enabled = true;
            gameObject.SetActive(false);
        }
        
        private void SpeedUp()
        {
            PlayerController.CurrentMoveSpeed += Time.deltaTime * timeMultiplier;
            if (PlayerController.CurrentMoveSpeed >= _upgradeMoveSpeed)
            {
                PlayerController.CurrentMoveSpeed = _upgradeMoveSpeed; 
                _isTimerStart = true;
            }
        }
        
        private void SlowDown()
        {
            PlayerController.CurrentMoveSpeed -= Time.deltaTime * timeMultiplier;
            if (PlayerController.CurrentMoveSpeed <= _defaultMoveSpeed)
            {
                DeactivateBuff();
            }
        }
        
        // !-- Helper/Utilites
        private void SetSpeedLimitValues(float nerfedMultiplier = 0f)
        {
            _defaultMoveSpeed = PlayerController.DefaultMoveSpeed - nerfedMultiplier;
            _upgradeMoveSpeed = _defaultMoveSpeed * speedUpMultiplier;
        }

        #endregion

        #region Effect Callbacks

        private void StartSpeedEffect()
        {
            StartCoroutine(StartSpeedEffectRoutine());
        }

        private void StopSpeedEffect()
        {
            _playerSpriteRenderer.color = defaultColor;
        }
        
        private IEnumerator StartSpeedEffectRoutine()
        {
            while (IsBuffActive)
            {
                yield return new WaitForSeconds(flashDuration);
                _playerSpriteRenderer.color  = flashColor;
                
                yield return new WaitForSeconds(flashDuration);
                _playerSpriteRenderer.color = defaultColor;
            }
        }

        #endregion
        
    }
}