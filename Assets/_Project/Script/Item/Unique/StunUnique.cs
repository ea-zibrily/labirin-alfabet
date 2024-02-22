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

        private float _currentTime;
        private bool _isItemThrowed;
        private bool _isCollideWithAnother;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            InitializeStun();
        }

        private void Update()
        {
            if (!_isItemThrowed || _isCollideWithAnother) return;
            
            Debug.Log("go stun");
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            StartCoroutine(HitOtherRoutine(gameObject));

            _currentTime += Time.deltaTime;
            if (_currentTime >= stunDuration)
            {
                _currentTime = 0;
                InitializeStun();
                gameObject.SetActive(false);
            }
        }

        #endregion

        #region Labirin Kata Callbacks

        private void InitializeStun()
        {
            _isCollideWithAnother = false;
            _isItemThrowed = false;
        }

        public void ThrowItem()
        {
            _isItemThrowed = true;
        }
        
        private IEnumerator HitOtherRoutine(GameObject otherObject)
        {
            var stunEffectObject = Instantiate(hitEffect, transform, worldPositionStays: false);
            stunEffectObject.transform.position = otherObject.transform.position;

            yield return new WaitForSeconds(stunDuration);
            Destroy(stunEffectObject);
            InitializeStun();
            gameObject.SetActive(false);
        }

        #endregion

        #region Collision Callbacks

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isItemThrowed || other.CompareTag("Player")) return;

            _isCollideWithAnother = true;
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            if (other.TryGetComponent<EnemyBase>(out EnemyBase enemy))
            {
                enemy.StopMovement();
                StartCoroutine(HitOtherRoutine(other.gameObject));
                enemy.StartMovement();
            }
            else
            {
                StartCoroutine(HitOtherRoutine(other.gameObject));
            }
        }

        #endregion
    }
}
