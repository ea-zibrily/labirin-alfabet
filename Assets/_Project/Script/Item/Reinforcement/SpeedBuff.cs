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
        
        private bool _isTimerStart;
        private bool _isSpeedUpComplete;
        
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
            
            _isTimerStart = false;
            _isSpeedUpComplete = false;
        }
        
        //-- Core Functionality
        public void Taken()
        {
            ActivateBuff();
        }

        protected override void ActivateBuff()
        {
            base.ActivateBuff();
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }

        protected override void DeactivateBuff()
        {
            base.DeactivateBuff();
            Debug.Log($"{gameObject.name} is deactive brok");
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
            gameObject.SetActive(false);
        }

        public override void BuffComplete()
        {
            base.BuffComplete();
            PlayerController.CurrentMoveSpeed = _normalMoveSpeed;
            DeactivateBuff();
            Debug.LogWarning("buff complete");
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
                BuffComplete();
            }
        }
        
        #endregion
    }
}