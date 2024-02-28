using System.Collections;
using System.Collections.Generic;
using LabirinKata.Entities.Enemy;
using UnityEngine;

namespace LabirinKata.Item
{
    public class StunUnique : MonoBehaviour
    {
        #region Fields & Property
        
        [Header("Stun Unique")]
        [SerializeField] private float stunDuration;
        [SerializeField] private GameObject hitEffect;

        private bool _isItemThrowed;
        
        public bool IsCollideWithAnother { get; private set; }

        [Header("Reference")]
        private CapsuleCollider2D _capsuleCollider;
        public SpriteRenderer SpriteRenderer { get; set; }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            InitializeStun();
        }

        #endregion

        #region Labirin Kata Callbacks

        private void InitializeStun()
        {
            _isItemThrowed = false;
            IsCollideWithAnother = false;
            _capsuleCollider.isTrigger = false;
        }

        public void ThrowItem()
        {
            _capsuleCollider.isTrigger = true;
            _isItemThrowed = true;
        }
        
        public void MissOther()
        {
            StartCoroutine(HitOtherRoutine(gameObject));
        }

        private IEnumerator HitEnemyRoutine(EnemyBase enemy)
        {
            enemy.StopMovement();
            StartCoroutine(HitOtherRoutine(enemy.gameObject));

            yield return new WaitForSeconds(stunDuration);
            // yield return HitOtherRoutine(enemy.gameObject);
            enemy.StartMovement();
        }

        private IEnumerator HitOtherRoutine(GameObject otherObject)
        {
            SpriteRenderer.enabled = false;
            var stunEffectObject = Instantiate(hitEffect, transform, worldPositionStays: false);
            stunEffectObject.transform.position = otherObject.transform.position;

            yield return new WaitForSeconds(stunDuration);
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

            Debug.Log("iscollide");
            IsCollideWithAnother = true;
            SpriteRenderer.sortingOrder--;

            if (other.TryGetComponent(out EnemyBase enemy))
            {
                Debug.Log("enemy");
                StartCoroutine(HitEnemyRoutine(enemy));
            }
            else
            {
                StartCoroutine(HitOtherRoutine(gameObject));
            }

            InitializeStun();
        }
        
        #endregion
    }
}
