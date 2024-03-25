using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Alphabet.Item;

namespace Alphabet.Entities.Player
{
    [AddComponentMenu("Alphabet/Entities/Player/Player Pick Throw")]
    public class PlayerPickThrow : MonoBehaviour
    {
        #region Fields & Property
        
        [Header("Pick")]
        [Range(0f, 1.5f)]
        [SerializeField] private float nerfedSpeedMultiplier;
        [SerializeField] private float pickAreaRadius;
        [SerializeField] private LayerMask itemLayerMask;
        [SerializeField] private Vector3 pickAreaMultiplier;

        private float _normalMoveSpeed;
        private GameObject _pickItemObject;
        private GameObject _holdedItemObject;

        public bool IsThrowItem { get; set; }
        public bool IsHoldedItem => _holdedItemObject != null;

        public event Action<float> OnPlayerInteract;

        public float NerfedMultiplier => nerfedSpeedMultiplier;
        public Vector3 PickDirection { get; set; }

        [Header("Throw")]
        [SerializeField] private float throwDelayDuration;
        [SerializeField] private float throwSpeed;
        
        [Header("UI")]
        [SerializeField] private Button interactButtonUI;

        [Header("Reference")]
        private PlayerController _playerController;
        

        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void Start()
        {
            InitializePickThrow();
        }

        private void Update()
        {
            if (_holdedItemObject)
            {
                var throwPosition =  transform.position + pickAreaMultiplier + PickDirection;
                _holdedItemObject.transform.position = throwPosition;
                interactButtonUI.gameObject.SetActive(true);
            }
            else
            {
                var pickAreaCollder = Physics2D.OverlapCircle(transform.position + PickDirection, pickAreaRadius, itemLayerMask);
                if (pickAreaCollder && pickAreaCollder.CompareTag("Pick"))
                {
                    if (pickAreaCollder.GetComponent<StunUnique>().IsItemThrowed) return;
                    
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

        #region Methods

        // !-- Initialization
        private void InitializePickThrow()
        {
            _holdedItemObject = null;
            _normalMoveSpeed = _playerController.DefaultMoveSpeed;
            PickDirection = Vector3.zero;
            IsThrowItem = false;
            
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
            _holdedItemObject.transform.parent = transform;

            _playerController.CurrentMoveSpeed -= nerfedSpeedMultiplier;
            OnPlayerInteract?.Invoke(nerfedSpeedMultiplier);

            if (!_pickItemObject.TryGetComponent(out StunUnique stunItem)) return;
            stunItem.GetComponent<Rigidbody2D>().simulated = false;
            stunItem.DisableSprite();
        }

        private void ThrowItem()
        {
            IsThrowItem = true;
            // _playerController.PlayerAnimator.SetBool(PlayerController.IS_HOLD, false);
        }

        public void CallThrowItem()
        {
            Debug.Log("thorw!");
            var item = _holdedItemObject;
            if (!item.TryGetComponent<StunUnique>(out var stunItem)) return;

            _playerController.StopMovement();

            stunItem.EnableSprite();
            stunItem.GetComponent<Rigidbody2D>().simulated = true;
            stunItem.ThrowItem(PickDirection, throwSpeed);

            _playerController.CurrentMoveSpeed = _normalMoveSpeed;
            OnPlayerInteract?.Invoke(0f);
            _playerController.StartMovement();
            _holdedItemObject = null;
        }

        // private IEnumerator ThrowItemRoutine(GameObject item)
        // {
        //     if (!item.TryGetComponent<StunUnique>(out var stunItem)) yield break;

        //     _playerController.StopMovement();
        //     yield return new WaitForSeconds(throwDelayDuration);

        //     stunItem.GetComponent<Rigidbody2D>().simulated = true;
        //     stunItem.ThrowItem(PickDirection, throwSpeed);

        //     _playerController.CurrentMoveSpeed = _normalMoveSpeed;
        //     OnPlayerInteract?.Invoke(0f);
        //     _playerController.StartMovement();
        // }

        // !-- Helper/Utilities
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position + PickDirection, pickAreaRadius);
            Gizmos.color = Color.red;
        }
        
        #endregion
    }
}
