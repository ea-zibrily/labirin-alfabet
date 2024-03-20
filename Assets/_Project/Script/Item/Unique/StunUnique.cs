using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alphabet.Entities.Enemy;
using Alphabet.Entities.Player;

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

        private bool _isCollideWithAnother;
        public bool IsItemThrowed { get; private set;}

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
            IsItemThrowed = false;
            _isCollideWithAnother = false;
            _capsuleCollider.isTrigger = false;
        }

        // !-- Core Functioanlity
        public void ThrowItem(Vector2 direction, float speed)
        {
             StartCoroutine(ThrowItemRoutine(direction, speed));
        }
        
        private IEnumerator ThrowItemRoutine(Vector2 direction, float speed)
        {
            IsItemThrowed = true;
            _capsuleCollider.isTrigger = true;
            var rotateDirectionSpeed  = direction.x > 0 ? -rotateEffectSpeed : rotateEffectSpeed;
            
            while (!_isCollideWithAnother)
            {
                transform.Translate(speed * Time.deltaTime * direction);
                _spriteRenderer.transform.Rotate(Vector3.forward * rotateDirectionSpeed);
                yield return null;
            }
        }
        
        private IEnumerator HitEnemyRoutine(EnemyBase enemy)
        {
            var enemyManager = enemy.GetComponent<EnemyManager>();
            enemyManager.EnemyStunnedEvent(stunDuration);
            enemyManager.DeactivateTrigger();
            enemy.StopMovement();

            yield return HitOtherRoutine(enemy.gameObject);
            enemyManager.ActivateTrigger();
            enemy.StartMovement();
        }
        
        private IEnumerator HitOtherRoutine(GameObject otherObject)
        {
            _spriteRenderer.enabled = false;
            _capsuleCollider.enabled = false;
            
            var stunEffectObject = Instantiate(hitEffect, effectParent.transform, worldPositionStays: false);
            stunEffectObject.transform.position = otherObject.transform.position;

            yield return new WaitForSeconds(stunDuration);
            Destroy(stunEffectObject);
            Destroy(gameObject);
        }

        // !-- Helper/Utilities
        public void EnableSprite() => _spriteRenderer.enabled = true;
        public void DisableSprite() => _spriteRenderer.enabled = false;

        private bool CheckColliderTag(Collider2D other)
        {
            return other.CompareTag("Wall") || other.CompareTag("Enemy");
        }

        #endregion

        #region Collision Callbacks

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsItemThrowed) return;
            if (!CheckColliderTag(other)) return;
            
            _isCollideWithAnother = true;
            if (other.TryGetComponent(out EnemyBase enemy))
            {
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
