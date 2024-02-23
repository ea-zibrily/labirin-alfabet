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
        public SpriteRenderer SpriteRenderer { get; set; }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
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
            IsCollideWithAnother = false;
            _isItemThrowed = false;
        }

        public void ThrowItem()
        {
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
            return other.CompareTag("Player") || !other.CompareTag("Wall") || !other.CompareTag("Enemy");
        }

        #endregion

        #region Collision Callbacks

        private void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: ganti penentu tag ke benda physisc (donn)
            if (!_isItemThrowed || CheckColliderTag(other)) return;

            IsCollideWithAnother = true;
            SpriteRenderer.sortingOrder--;

            if (other.TryGetComponent(out EnemyBase enemy))
            {
                Debug.LogWarning($"go stun when collide w {enemy.name}");
                StartCoroutine(HitEnemyRoutine(enemy));
            }
            else
            {
                Debug.LogWarning($"go stun when collide w {other.gameObject.name}");
                StartCoroutine(HitOtherRoutine(gameObject));
            }

            _isItemThrowed = false;
        }

        #endregion
    }
}
