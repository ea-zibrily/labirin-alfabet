using System;
using System.Collections;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Data;

namespace LabirinKata.Entities.Player
{
    [AddComponentMenu("Labirin Kata/Entities/Player/Player Controller")]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerController : MonoBehaviour
    {
        #region Constant Variable

        private const string HORIZONTAL_KEY = "Horizontal";
        private const string VERTICAL_KEY = "Vertical";
        private const string IS_MOVE = "isMove";

        #endregion
        
        #region Fields & Properties
        
        [Header("Player")] 
        [SerializeField] private PlayerData playerData;
        [SerializeField] [ReadOnly] private float currentMoveSpeed;
        [SerializeField] private Vector2 movementDirection;
        
        public float DefaultMoveSpeed => playerData.MoveSpeed;
        public float CurrentMoveSpeed
        {
            get => currentMoveSpeed;
            set => currentMoveSpeed = value;
        }
        
        public bool CanMove { get; private set; }
        
        [Header("Reference")] 
        private Rigidbody2D _playerRb;
        private Animator _playerAnimator;
        private PlayerPickThrow _playerPickThrow;

        public PlayerInputHandler PlayerInputHandler { get; private set; }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            _playerAnimator = GetComponentInChildren<Animator>();
            _playerPickThrow = GetComponentInChildren<PlayerPickThrow>();
            PlayerInputHandler = GetComponentInChildren<PlayerInputHandler>();
        }

        private void Start()
        {
            InitializePlayer();
            StartCoroutine(StartPlayerMove());
        }

        private void FixedUpdate()
        {
            PlayerMove();
        }

        private void Update()
        {
            PlayerAnimation();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        private void InitializePlayer()
        {
            gameObject.name = playerData.PlayerName;
            CurrentMoveSpeed = playerData.MoveSpeed;
        }
        
        private IEnumerator StartPlayerMove()
        {
            StopMovement();
            yield return new WaitForSeconds(1f);
            StartMovement();
        }
        
        // !-- Core Functionality
        private void PlayerMove()
        {
            if (!CanMove) return;
            
            //*-- W Enhanced Touch Input
            var moveX = PlayerInputHandler.Direction.x;
            var moveY = PlayerInputHandler.Direction.y;
            
            if (Mathf.Abs(moveX) > Mathf.Abs(moveY))
            {
                moveY = 0f;
            }
            else
            {
                moveX = 0f;
            }
            
            movementDirection = new Vector2(moveX, moveY);
            movementDirection.Normalize();
            if (movementDirection.sqrMagnitude > 0.5f)
            {
                _playerPickThrow.PickDirection = movementDirection;
            }
            
            _playerRb.velocity = movementDirection * CurrentMoveSpeed;
        }
        
        private void PlayerAnimation()
        {
            if (movementDirection != Vector2.zero)
            {
                _playerAnimator.SetFloat(HORIZONTAL_KEY, movementDirection.x);
                _playerAnimator.SetFloat(VERTICAL_KEY, movementDirection.y);
                _playerAnimator.SetBool(IS_MOVE, true);
            }
            else
            {
                _playerAnimator.SetBool(IS_MOVE, false);
            }
        }
        
        // !-- Helpers/Utilities
        public void StartMovement()
        {
            CanMove = true;
            PlayerInputHandler.EnableTouchInput();
        }
        
        public void StopMovement()
        {
            CanMove = false;
            _playerRb.velocity = Vector2.zero;
            movementDirection = Vector2.zero;
            PlayerInputHandler.DisableTouchInput();
        }
        
        public void SetPlayerDirection(Transform value)
        {
            var direction = value.transform.position - transform.position;
            direction.Normalize();

            _playerAnimator.SetFloat(HORIZONTAL_KEY, direction.x);
            _playerAnimator.SetFloat(VERTICAL_KEY, direction.y);
        }
        
        #endregion
    }
}
