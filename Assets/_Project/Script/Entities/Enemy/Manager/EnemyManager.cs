using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alphabet.Gameplay.EventHandler;
using KevinCastejon.MoreAttributes;
using UnityEngine.UI;
using JetBrains.Annotations;
using System;

namespace Alphabet.Entities.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        #region Fields & Property

        [Header("Stun")]
        [SerializeField] private GameObject stunPanelUI;
        [SerializeField] private Image stunFillBarUI;
        
        // Const Variable
        private const float MAX_FILL_BAR = 1f;
        private const float MIN_FILL_BAR = 0f;
        
        // Event
        public event Action<float> OnEnemyStunned;

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
            CameraEventHandler.OnCameraShiftIn += _enemyBase.StopMovement;
            CameraEventHandler.OnCameraShiftOut += _enemyBase.StartMovement;

            // Game
            GameEventHandler.OnGameStart += _enemyBase.StartMovement;

            // Feedback
            OnEnemyStunned += UpdateStunBar;
        }
        
        private void OnDisable()
        {
            // Camera
            CameraEventHandler.OnCameraShiftIn -= _enemyBase.StopMovement;
            CameraEventHandler.OnCameraShiftOut -= _enemyBase.StartMovement;

            // Game
            GameEventHandler.OnGameStart -= _enemyBase.StartMovement;

            // Feedback
            OnEnemyStunned -= UpdateStunBar;
        }

        private void Start()
        {
            ActivateTrigger();
            InitializeStunBar();
            _enemyBase.StopMovement();
        }

        #endregion

        #region Stun Feedback Methods

        // !-- Initialization
        private void InitializeStunBar()
        {
            stunFillBarUI.fillAmount = MAX_FILL_BAR;
            stunPanelUI.SetActive(false);
        }

        // !-- Core Functioanlity
        public void EnemyStunnedEvent(float duration) => OnEnemyStunned?.Invoke(duration);

        private void UpdateStunBar(float duration)
        {
            stunFillBarUI.fillAmount = MAX_FILL_BAR;
            stunPanelUI.SetActive(true);

            StartCoroutine(DecreaseBarRoutine(duration));
        }

        private IEnumerator DecreaseBarRoutine(float duration)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                stunFillBarUI.fillAmount = Mathf.Lerp(MAX_FILL_BAR, MIN_FILL_BAR, elapsedTime / duration);
                yield return null;
            }

            stunFillBarUI.fillAmount = MIN_FILL_BAR;
            stunPanelUI.SetActive(false);
        }

        #endregion

        #region Collision Methods

        // !-- Helper/Utilities
        public void ActivateTrigger() => _capsuleCollider.isTrigger = true;
        public void DeactivateTrigger() => _capsuleCollider.isTrigger = false;

        #endregion
    }
}
