using System;
using System.Collections;
using UnityEngine;
using Spine;
using Spine.Unity;
using Alphabet.Enum;
using Alphabet.Managers;
using Alphabet.Entities.Player;
using Alphabet.Gameplay.EventHandler;
using Alphabet.Gameplay.Controller;

namespace Alphabet.Item
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class SpeedBuff : BuffItem, ITakeable
    {
        #region Fields & Properties

        [Header("Speed Buff")]
        [Range(0f, 2.5f)]
        [SerializeField] private float speedUpMultiplier;
        [SerializeField] private float timeDuration;
        [Range(0.5f, 1.5f)]
        [SerializeField] private float timeMultiplier;
        
        private float _upgradeMoveSpeed;
        private float _defaultMoveSpeed;
        private float _currentTime;
        private bool _isCameraShift;
        
        [Header("Speed Effect")]
        [SerializeField] private float flashDuration;
        [SerializeField] private Color flashColor;
        
        [Header("Reference")]
        private Skeleton _playerSkeleton;
        private PlayerPickThrow _playerPickThrow;
        private PlayerFlash _playerFlash;

        #endregion
        
        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            // Speed
            _playerPickThrow.OnPlayerInteract += SetSpeedLimitValues;

            // Camera
            CameraEventHandler.OnCameraShiftIn += CameraShiftInEvent;
            CameraEventHandler.OnCameraShiftOut += CameraShiftOutEvent;
        }

        private void OnDisable()
        {
            // Speed
            _playerPickThrow.OnPlayerInteract -= SetSpeedLimitValues;

            // Camera
            CameraEventHandler.OnCameraShiftIn -= CameraShiftInEvent;
            CameraEventHandler.OnCameraShiftOut -= CameraShiftOutEvent;
        }

        #endregion

        #region Methods

        // !-- Initialization
        protected override void InitializeOnAwake()
        {
            base.InitializeOnAwake();

            _playerPickThrow = PlayerController.GetComponent<PlayerPickThrow>();
            _playerSkeleton = PlayerController.GetComponentInChildren<SkeletonAnimation>().Skeleton;
            _playerFlash = new PlayerFlash(flashColor, flashDuration, _playerSkeleton);
        }

        private void InitializeSpeed()
        {
            _defaultMoveSpeed = PlayerController.CurrentMoveSpeed;
            _upgradeMoveSpeed = _defaultMoveSpeed * speedUpMultiplier;
        }
        
        // !-- Core Functionality
        public void Taken()
        {
            if (PlayerManager.HasBuffEffect[BuffType]) return;
            ActivateBuff();
        }
        
        protected override void ActivateBuff()
        {
            base.ActivateBuff();
            
            InitializeSpeed();
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            
            StartCoroutine(SpeedActive());
            StartSpeedEffect(PlayerManager.SpeedEffect);
            FindObjectOfType<AudioManager>().PlayAudio(Musics.SpeedSfx);
        }

        public override void DeactivateBuff()
        {
            base.DeactivateBuff();

            StopSpeedEffect(PlayerManager.SpeedEffect);
            PlayerController.CurrentMoveSpeed = _defaultMoveSpeed;

            gameObject.SetActive(false);
        }

        private IEnumerator SpeedActive()
        {
            yield return SpeedUpRoutine();

            while (_currentTime < timeDuration)
            {
                if (!_isCameraShift)
                {
                    _currentTime += Time.deltaTime; 
                }
                yield return null;
            }
            _currentTime = 0;
            yield return SlowDownRoutine();
        }

        private IEnumerator SpeedUpRoutine()
        {
            while (PlayerController.CurrentMoveSpeed < _upgradeMoveSpeed)
            {
                if (!_isCameraShift)
                {
                    PlayerController.CurrentMoveSpeed += Time.deltaTime * timeMultiplier;
                }
                yield return null;
            }
            PlayerController.CurrentMoveSpeed = _upgradeMoveSpeed; 
        }

        private IEnumerator SlowDownRoutine()
        {
            while (PlayerController.CurrentMoveSpeed > _defaultMoveSpeed)
            {
                if (!_isCameraShift)
                {
                    PlayerController.CurrentMoveSpeed -= Time.deltaTime * timeMultiplier;
                }
                yield return null;
            }
            DeactivateBuff();
        }
        
        // !-- Helper/Utilites
        private void SetSpeedLimitValues(float nerfedMultiplier = 0f)
        {
            _defaultMoveSpeed = PlayerController.DefaultMoveSpeed - nerfedMultiplier;
            _upgradeMoveSpeed = _defaultMoveSpeed * speedUpMultiplier;
        }

        #endregion

        #region Extend Methods

        // Event
        private void CameraShiftInEvent() => _isCameraShift = true;
        private void CameraShiftOutEvent() => _isCameraShift = false;

        // Effect
        private void StartSpeedEffect(GameObject buffEffect)
        {
            StartCoroutine(_playerFlash.FlashWithConditionRoutine(IsBuffActive));

            if (!buffEffect.TryGetComponent<ParticleController>(out var effect)) return;
            effect.PlayParticle();
        }

        private void StopSpeedEffect(GameObject buffEffect)
        {
            StopCoroutine(_playerFlash.FlashWithConditionRoutine(IsBuffActive));
            _playerSkeleton.SetColor(Color.white);

            if (!buffEffect.TryGetComponent<ParticleController>(out var effect)) return;
            effect.StopParticle();
        }

        #endregion

    }
}