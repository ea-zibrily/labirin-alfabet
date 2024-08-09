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
        [SerializeField] private Slider stunBarSliderUI;
        [SerializeField] [ReadOnly] private bool _isStunned;

        public bool IsStunned => _isStunned;

        // Const Variable
        private const float MAX_FILL_BAR = 1f;
        private const float MIN_FILL_BAR = 0f;

        // Reference
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
            GameEventHandler.OnGameWin += () => _enemyBase.StopMovement();
            GameEventHandler.OnGameOver += value => _enemyBase.StopMovement();
        }
        
        private void OnDisable()
        {
            // Camera
            CameraEventHandler.OnCameraShiftIn -= CameraShiftInEvent;
            CameraEventHandler.OnCameraShiftOut -= CameraShiftOutEvent;

            // Game
            GameEventHandler.OnGameStart -= _enemyBase.StartMovement;
            GameEventHandler.OnGameWin -= () => _enemyBase.StopMovement();
            GameEventHandler.OnGameOver -= value => _enemyBase.StopMovement();
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

        // !- Initialize
        private void InitializeStunBar()
        {
            _isStunned = false;
            stunBarSliderUI.gameObject.SetActive(false);
            stunBarSliderUI.value = MAX_FILL_BAR;
        }

        // !- Core
        public void PerformStunBar()
        {
            stunBarSliderUI.value = MAX_FILL_BAR;
            stunBarSliderUI.gameObject.SetActive(true);
            _isStunned = true;
        }

        public void DecreaseStunBar(float duration, float elapsedTime)
        {
            stunBarSliderUI.value = Mathf.Lerp(MAX_FILL_BAR, MIN_FILL_BAR, elapsedTime / duration);
            if (stunBarSliderUI.value <= MIN_FILL_BAR)
            {
                _isStunned = false;
                stunBarSliderUI.gameObject.SetActive(false);
                stunBarSliderUI.value = MAX_FILL_BAR;
            }
        }

        // !- Helper
        public void ActivateTrigger() => _capsuleCollider.isTrigger = true;
        public void DeactivateTrigger() => _capsuleCollider.isTrigger = false;

        #endregion
    }
}
