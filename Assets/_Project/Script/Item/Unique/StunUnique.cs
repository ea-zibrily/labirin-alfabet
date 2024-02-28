using System.Collections;
using System.Collections.Generic;
using LabirinKata.Entities.Enemy;
using LabirinKata.Entities.Player;
using UnityEngine;

namespace LabirinKata.Item
{
    public class StunUnique : MonoBehaviour
    {
        #region Fields & Property
        
        [Header("Stun Unique")]
        [SerializeField] private float stunDuration;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private GameObject effectParent;

        private bool _isItemThrowed;
        public bool IsCollideWithAnother { get; private set; }

        [Header("Reference")]
        private CapsuleCollider2D _capsuleCollider;
        private SpriteRenderer _spriteRenderer;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            InitializeStun();
        }
        
        #endregion

        #region Methods

        private void InitializeStun()
        {
            _isItemThrowed = false;
            IsCollideWithAnother = false;
            _capsuleCollider.isTrigger = false;
        }
        
        // !-- Core Functioanlity
        public void ThrowItem(Vector2 direction, float speed)
        {
             StartCoroutine(ThrowItemRoutine(direction, speed));
        }

        private IEnumerator ThrowItemRoutine(Vector2 direction, float speed)
        {
            _isItemThrowed = true;
            _capsuleCollider.isTrigger = true;

            while (!IsCollideWithAnother)
            {
                transform.Translate(speed * Time.deltaTime * direction);
                yield return null;
            }
        }
        
        private IEnumerator HitEnemyRoutine(EnemyBase enemy)
        {
            enemy.StopMovement();

            yield return HitOtherRoutine(enemy.gameObject);
            enemy.StartMovement();
        }

        private IEnumerator HitOtherRoutine(GameObject otherObject)
        {
            _isItemThrowed = false;
            _spriteRenderer.enabled = false;
            
            var stunEffectObject = Instantiate(hitEffect, effectParent.transform, worldPositionStays: false);
            stunEffectObject.transform.position = otherObject.transform.position;

            yield return new WaitForSeconds(stunDuration);
            Destroy(stunEffectObject);
            Destroy(gameObject);
        }

        // !-- Helper/Utilities
        private bool CheckColliderTag(Collider2D other)
        {
            return other.CompareTag("Wall") || other.CompareTag("Enemy");
        }

        #endregion

        #region Collision Callbacks

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isItemThrowed) return;
            if (!CheckColliderTag(other)) return;

            IsCollideWithAnother = true;
            if (other.TryGetComponent(out EnemyBase enemy))
            {
                Debug.Log("enemy");
                StartCoroutine(HitEnemyRoutine(enemy));
            }
            else
            {
                StartCoroutine(HitOtherRoutine(gameObject));
            }
        }
        
        #endregion
    }
}
