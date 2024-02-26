using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LabirinKata.Item;

namespace LabirinKata.Entities.Player
{
    [AddComponentMenu("Labirin Kata/Entities/Player/Player Pick Throw")]
    public class PlayerPickThrow : MonoBehaviour
    {
        #region Fields & Property
        
        [Header("Pick")]
        [SerializeField] private Transform pickAreaTransform;
        [SerializeField] private float pickAreaRadius;
        [SerializeField] private LayerMask itemLayerMask;

        private GameObject _pickItemObject;
        private GameObject _holdedItemObject;

        public Vector3 PickDirection { get; set; }

        [Header("Throw")]
        [SerializeField] private float throwRange;
        [SerializeField] private float throwDuration;
        [SerializeField] private float throwDelay;
        
        [Header("UI")]
        [SerializeField] private Button interactButtonUI;
        

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            InitializePickThrow();
        }

        private void Update()
        {
            if (_holdedItemObject)
            {
                interactButtonUI.gameObject.SetActive(true);
            }
            else
            {
                var pickAreaCollder = Physics2D.OverlapCircle(transform.position + PickDirection, pickAreaRadius, itemLayerMask);
                if (pickAreaCollder && pickAreaCollder.CompareTag("Pick"))
                {
                    _pickItemObject = pickAreaCollder.gameObject;
                    interactButtonUI.gameObject.SetActive(true);
                }
                else
                {
                    interactButtonUI.gameObject.SetActive(false);
                }
            }
        }
        
        #endregion

        #region Labirin Kata Callbacks

        // !-- Initialization
        private void InitializePickThrow()
        {
            _holdedItemObject = null;
            PickDirection = Vector3.zero;
            
            interactButtonUI.onClick.AddListener(PickThrowItem);
            interactButtonUI.gameObject.SetActive(false);
        }

        // !-- Core Functionality
        private void PickThrowItem()
        {
            if (_holdedItemObject)
            {
                ThrowItem();
            }
            else
            {
                PickItem();
            }
        }

        private void PickItem()
        {
            _holdedItemObject = _pickItemObject;
            _holdedItemObject.transform.position = pickAreaTransform.transform.position;
            _holdedItemObject.transform.parent = transform;

            if (_pickItemObject.TryGetComponent(out StunUnique stunItem))
            {
                stunItem.SpriteRenderer.sortingOrder++;
                stunItem.GetComponent<Rigidbody2D>().simulated = false;
            }
        }

        private void ThrowItem()
        {
            StartCoroutine(ThrowItemRoutine(_holdedItemObject));
            _holdedItemObject = null;
        }

        private IEnumerator ThrowItemRoutine(GameObject item)
        {
            var elapsedTime = 0f;
            var startPoint = item.transform.position;
            var endPoint = transform.position + PickDirection * throwRange;

            // TODO: Drop logic to ref item script here!
            if (item.TryGetComponent<StunUnique>(out var stunItem))
            {
                stunItem.GetComponent<Rigidbody2D>().simulated = true;
                stunItem.ThrowItem();
            }

            while (elapsedTime < throwDelay)
            {   
                if (stunItem.IsCollideWithAnother) yield break;
                
                item.transform.position = Vector3.Lerp(startPoint, endPoint, elapsedTime * 0.04f);
                elapsedTime++;
                yield return null;
            }

            stunItem.MissOther();
        }

        // !-- Helper/Utilities
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position + PickDirection, pickAreaRadius);
            Gizmos.color = Color.red;
        }
        
        #endregion
    }
}
