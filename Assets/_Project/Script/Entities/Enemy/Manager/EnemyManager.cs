using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Alphabet.Gameplay.EventHandler;
using KevinCastejon.MoreAttributes;

namespace Alphabet.Entities.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        #region Fields & Property

        [Header("Stun")]
        [SerializeField] private GameObject stunPanelUI;
        [SerializeField] private Image stunFillBarUI;
        [SerializeField] [ReadOnly] private bool _isStunned;

        public bool IsStunned => _isStunned;

        
        // Const Variable
        private const float MAX_FILL_BAR = 1f;
        private const float MIN_FILL_BAR = 0f;

        [Header("Reference")]
        private EnemyBase _enemyBase;
        private CapsuleCollider2D _capsuleCollider;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _enemyBase = GetComponent<EnemyBase>();
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
        }

        private void OnEnable()
        {
            // Camera
            CameraEventHandler.OnCameraShiftIn += CameraShiftInEvent;
            CameraEventHandler.OnCameraShiftOut += CameraShiftOutEvent;

            // Game
            GameEventHandler.OnGameStart += _enemyBase.StartMovement;
        }
        
        private void OnDisable()
        {
            // Camera
            CameraEventHandler.OnCameraShiftIn -= CameraShiftInEvent;
            CameraEventHandler.OnCameraShiftOut -= CameraShiftOutEvent;

            // Game
            GameEventHandler.OnGameStart -= _enemyBase.StartMovement;
        }
        
        private void Start()
        {
            ActivateTrigger();
            InitializeStunBar();
        }

        #endregion

        #region Camera Methods

        // !-- Core Functionality
        private void CameraShiftInEvent()
        {
            if (IsStunned) return;
            _enemyBase.StopMovement();
        }

        private void CameraShiftOutEvent()
        {
            if (IsStunned) return;
            _enemyBase.StartMovement();
        }
        
        #endregion

        #region Stun Feedback Methods

        // !-- Initialization
        private void InitializeStunBar()
        {
            _isStunned = false;
            stunPanelUI.SetActive(false);
            stunFillBarUI.fillAmount = MAX_FILL_BAR;
        }

        // !-- Core Functioanlity
        public void PerformStunBar()
        {
            stunFillBarUI.fillAmount = MAX_FILL_BAR;
            stunPanelUI.SetActive(true);
            _isStunned = true;
        }

        public void DecreaseStunBar(float duration, float elapsedTime)
        {
            stunFillBarUI.fillAmount = Mathf.Lerp(MAX_FILL_BAR, MIN_FILL_BAR, elapsedTime / duration);
            if (stunFillBarUI.fillAmount <= MIN_FILL_BAR)
            {
                _isStunned = false;
                stunPanelUI.SetActive(false);
                stunFillBarUI.fillAmount = MAX_FILL_BAR;
            }
        }

        #endregion

        #region Collision Methods

        // !-- Helper/Utilities
        public void ActivateTrigger() => _capsuleCollider.isTrigger = true;
        public void DeactivateTrigger() => _capsuleCollider.isTrigger = false;

        #endregion
    }
}
