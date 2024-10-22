using System;
using System.Collections;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using Alphabet.Data;
using Alphabet.Database;
using Alphabet.Gameplay.EventHandler;

namespace Alphabet.Entities.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerController : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Movement")] 
        [SerializeField] private PlayerData playerData;
        [SerializeField] [ReadOnly] private float currentMoveSpeed;
        [SerializeField] private Vector2 movementDirection;
        
        public Vector2 MovementDirection => movementDirection;
        public float DefaultMoveSpeed => playerData.PlayerMoveSpeed;
        public float CurrentMoveSpeed
        {
            get => currentMoveSpeed;
            set => currentMoveSpeed = value;
        }
        public bool CanMove { get; private set; }
        
        [Header("Reference")] 
        private Rigidbody2D _playerRb;
        private PlayerAnimation _playerAnimation;
        public PlayerPickThrow PlayerPickThrow { get; private set; }
        public JoystickInputHandler PlayerInputHandler { get; private set; }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            // Component
            _playerRb = GetComponent<Rigidbody2D>();

            // Handler
            _playerAnimation = GetComponentInChildren<PlayerAnimation>();
            PlayerPickThrow = GetComponent<PlayerPickThrow>();
            PlayerInputHandler = GetComponentInChildren<JoystickInputHandler>();
        }

        
        private void OnEnable()
        {
            GameEventHandler.OnGameStart += StartMovement;
        }

        private void OnDisable()
        {
            GameEventHandler.OnGameStart -= StartMovement;
        }

        private void Start()
        {
            InitPlayer();
        }

        private void FixedUpdate()
        {
            PlayerMove();
        }
        
        #endregion
        
        #region Methods
        
        // !- Initialization
        private void InitPlayer()
        {
            // Data
            playerData = PlayerDatabase.Instance.GetPlayerDatabySelected();

            // Component
            gameObject.name = playerData.PlayerName;
            CurrentMoveSpeed = playerData.PlayerMoveSpeed;
            _playerAnimation.ChangeSkinWhenPlay(playerData.PlayerSkin);
        }
        
        // !- Core Functionality
        private void PlayerMove()
        {
            if (!CanMove) return;
            
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
                PlayerPickThrow.PickDirection = movementDirection;
            }
            
            _playerRb.velocity = movementDirection * CurrentMoveSpeed;
        }
        
        // !- Utilities
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
        
        public void DefaultDirection()
        {
            movementDirection = Vector2.right;
            _playerAnimation.SetDefaultState();
            movementDirection = Vector2.zero;
        }
        
        public void SetDirection(Transform value)
        {
            var direction = value.transform.position - transform.position;
            direction.Normalize();
            
            movementDirection = direction;
            movementDirection = Vector2.zero;
        }
        
        #endregion
    }
}
