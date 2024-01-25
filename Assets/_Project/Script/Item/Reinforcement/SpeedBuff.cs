using System;
using UnityEngine;

namespace LabirinKata.Item.Reinforcement
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SpeedBuff : BuffItem, ITakeable
    {
        #region Variable

        [Header("Speed Buff")]
        [SerializeField] private float moveSpeedUp;
        [SerializeField] private float speedUpTimeDuration;
        [SerializeField] private float speedUpMultiplier;

        private float _normalMoveSpeed;
        private float _currentTime;
        
        private bool _isBuffActive;
        private bool _isTimerStart;
        private bool _isSpeedUpComplete;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            if (!_isBuffActive) return;
            
            if (!_isSpeedUpComplete)
            {
                SpeedUpPlayer();

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
                SlowDownPlayer();
            }
        }

        #endregion
        
        #region Labirin Kata Callbacks

        //-- Initialization
        protected override void InitializeOnStart()
        {
            base.InitializeOnStart();
            InitializeSpeedBuff();
        }
        
        private void InitializeSpeedBuff()
        {
            _normalMoveSpeed = PlayerController.DefaultMoveSpeed;
            _currentTime = 0;
            
            _isBuffActive = false;
            _isTimerStart = false;
            _isSpeedUpComplete = false;
        }
        
        //-- Core Functionality
        public void Taken()
        {
            ActivateBuff();
        }
        
        private void ActivateBuff()
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            _isBuffActive = true;
        }
        
        private void SpeedUpPlayer()
        {
            PlayerController.CurrentMoveSpeed += Time.deltaTime * speedUpMultiplier;
            if (PlayerController.CurrentMoveSpeed >= moveSpeedUp)
            {
                PlayerController.CurrentMoveSpeed = moveSpeedUp;
                _isTimerStart = true;
            }
        }
        
        private void SlowDownPlayer()
        {
            PlayerController.CurrentMoveSpeed -= Time.deltaTime * speedUpMultiplier;
            if (PlayerController.CurrentMoveSpeed <= _normalMoveSpeed)
            {
                PlayerController.CurrentMoveSpeed = _normalMoveSpeed;
                _isBuffActive = false;
                gameObject.SetActive(false);
            }
        }
        
        #endregion
    }
}