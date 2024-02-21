using System;
using System.Collections;
using UnityEngine;

namespace LabirinKata.Item
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SpeedBuff : BuffItem, ITakeable
    {
        #region Fields & Properties

        [Header("Speed Buff")]
        [SerializeField] private float moveSpeedUp;
        [SerializeField] private float speedUpTimeDuration;
        [SerializeField] private float speedUpMultiplier;
        
        [Header("Buff Effect")]
        [SerializeField] private float flashDuration;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color flashColor;

        private float _normalMoveSpeed;
        private float _currentTime;
        
        private bool _isTimerStart;
        private bool _isSpeedUpComplete;
        
        [Header("Reference")]
        private SpriteRenderer playerSpriteRenderer;

        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Update()
        {
            if (!IsBuffActive) return;
            
            if (!_isSpeedUpComplete)
            {
                SpeedUp();

                if (!_isTimerStart) return;
                
                _currentTime += Time.deltaTime;
                if (_currentTime >= speedUpTimeDuration)
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
            playerSpriteRenderer = PlayerController.GetComponentInChildren<SpriteRenderer>();
        }

        protected override void InitializeOnStart()
        {
            base.InitializeOnStart();
            InitializeSpeedBuff();
        }
        
        private void InitializeSpeedBuff()
        {
            _normalMoveSpeed = PlayerController.DefaultMoveSpeed;
            _currentTime = 0;
            
            _isTimerStart = false;
            _isSpeedUpComplete = false;
        }
        
        // !-- Core Functionality
        public void Taken()
        {
            ActivateBuff();
        }

        protected override void ActivateBuff()
        {
            base.ActivateBuff();

            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            StartSpeedEffect();
        }

        public override void DeactivateBuff()
        {
            base.DeactivateBuff();

            StopSpeedEffect();
            PlayerController.CurrentMoveSpeed = _normalMoveSpeed;
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
            gameObject.SetActive(false);
        }
        
        private void SpeedUp()
        {
            PlayerController.CurrentMoveSpeed += Time.deltaTime * speedUpMultiplier;
            if (PlayerController.CurrentMoveSpeed >= moveSpeedUp)
            {
                PlayerController.CurrentMoveSpeed = moveSpeedUp; 
                _isTimerStart = true;
            }
        }
        
        private void SlowDown()
        {
            PlayerController.CurrentMoveSpeed -= Time.deltaTime * speedUpMultiplier;
            if (PlayerController.CurrentMoveSpeed <= _normalMoveSpeed)
            {
                DeactivateBuff();
            }
        }

        private void StartSpeedEffect()
        {
            StartCoroutine(StartSpeedEffectRoutine());
        }
        
        private IEnumerator StartSpeedEffectRoutine()
        {
            while (IsBuffActive)
            {
                yield return new WaitForSeconds(flashDuration);
                playerSpriteRenderer.color  = flashColor;
                
                yield return new WaitForSeconds(flashDuration);
                playerSpriteRenderer.color = defaultColor;
            }
        }

        private void StopSpeedEffect()
        {
            playerSpriteRenderer.color = defaultColor;
        }

        #endregion
    }
}