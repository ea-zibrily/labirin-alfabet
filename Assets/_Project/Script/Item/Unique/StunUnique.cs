using System.Collections;
using UnityEngine;
using Alphabet.Enum;
using Alphabet.Managers;
using Alphabet.Entities.Enemy;
using Alphabet.Gameplay.EventHandler;
using Alphabet.Gameplay.Controller;

namespace Alphabet.Item
{
    public class StunUnique : MonoBehaviour
    {
        #region Fields & Property
        
        [Header("Stun Unique")]
        [SerializeField] private float stunDuration;
        [SerializeField] private float rotateEffectSpeed;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private GameObject effectParent;
        [SerializeField] private GameObject dustEffect;
        
        private float _elapsedTime;
        private bool _isCameraShift;
        private bool _isCollideWithAnother;

        public GameObject DustEffect => dustEffect;
        public bool IsItemThrowed { get; private set;}

        [Header("Reference")]
        private CircleCollider2D _stunCollider;
        private SpriteRenderer _spriteRenderer;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _stunCollider = GetComponent<CircleCollider2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            // Camera
            CameraEventHandler.OnCameraShiftIn += CameraShiftInEvent;
            CameraEventHandler.OnCameraShiftOut += CameraShiftOutEvent;
        }

        private void OnDisable()
        {
            // Camera
            CameraEventHandler.OnCameraShiftIn -= CameraShiftInEvent;
            CameraEventHandler.OnCameraShiftOut -= CameraShiftOutEvent;
        }

        private void Start()
        {
            InitializeStun();
        }
        
        #endregion

        #region Methods

        // !- Initialize
        private void InitializeStun()
        {
            _elapsedTime = 0f;
            _isCollideWithAnother = false;
            _stunCollider.isTrigger = false;
            dustEffect.SetActive(false);

            IsItemThrowed = false;
        }

        // !- Core
        public void ThrowItem(Vector2 direction, float speed)
        {
            StartCoroutine(ThrowItemRoutine(direction, speed));
        }
        
        private IEnumerator ThrowItemRoutine(Vector2 direction, float speed)
        {
            IsItemThrowed = true;
            _stunCollider.isTrigger = true;
            var rotateDirectionSpeed  = direction.x > 0 ? -rotateEffectSpeed : rotateEffectSpeed;
            
            while (!_isCollideWithAnother)
            {
                transform.Translate(speed * Time.deltaTime * direction);
                _spriteRenderer.transform.Rotate(Vector3.forward * rotateDirectionSpeed);
                yield return null;
            }
        }

        private IEnumerator PerformStunEffect(EnemyBase enemy)
        {
            var otherObject = enemy.gameObject;
            var enemyManager = enemy.GetComponent<EnemyManager>();

            _spriteRenderer.enabled = false;
            _stunCollider.enabled = false;
            
            var stunEffectObject = Instantiate(hitEffect, effectParent.transform, worldPositionStays: false);
            stunEffectObject.transform.position = otherObject.transform.position;

            enemy.StopMovement();
            enemyManager.PerformStunBar();
            enemyManager.DeactivateTrigger();
            while (_elapsedTime < stunDuration)
            {
                if (!_isCameraShift)
                {
                    _elapsedTime += Time.deltaTime;
                    enemyManager.DecreaseStunBar(stunDuration, _elapsedTime);
                }
                yield return null;
            }

            enemyManager.ActivateTrigger();
            enemy.StartMovement();
            Destroy(stunEffectObject);
            Destroy(gameObject);
        }
        
        private IEnumerator PerformHitEffect(GameObject otherObject)
        {
            _spriteRenderer.enabled = false;
            _stunCollider.enabled = false;
            
            var stunEffectObject = Instantiate(hitEffect, effectParent.transform, worldPositionStays: false);
            stunEffectObject.transform.position = otherObject.transform.position;

            while (_elapsedTime < stunDuration)
            {
                if (!_isCameraShift)
                {
                    _elapsedTime += Time.deltaTime;
                }
                yield return null;
            }

            Destroy(stunEffectObject);
            Destroy(gameObject);
        }

        // !- Helper
        public void EnableSprite() => _spriteRenderer.enabled = true;
        public void DisableSprite() => _spriteRenderer.enabled = false;

        private bool CheckColliderTag(Collider2D other)
        {
            return other.CompareTag("Wall") || other.CompareTag("Enemy");
        }

        #endregion

        #region Camera Methods

        private void CameraShiftInEvent() => _isCameraShift = true;
        private void CameraShiftOutEvent() => _isCameraShift = false;

        #endregion

        #region Collision Callbacks

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsItemThrowed) return;
            if (!CheckColliderTag(other)) return;
            
            _isCollideWithAnother = true;
            if (dustEffect.TryGetComponent<ParticleController>(out var effect)) effect.StopParticle();
            FindObjectOfType<AudioManager>().PlayAudio(Musics.StonebreakSfx);
            
            if (other.TryGetComponent(out EnemyBase enemy))
            {
                StartCoroutine(PerformStunEffect(enemy));
            }
            else
            {
                StartCoroutine(PerformHitEffect(gameObject));
            }
        }
        
        #endregion
    }
}
