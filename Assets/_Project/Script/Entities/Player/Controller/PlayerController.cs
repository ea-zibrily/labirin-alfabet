using System;
using System.Collections;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using Alphabet.Data;
using Alphabet.Database;
using Spine.Unity;
using Spine;

namespace Alphabet.Entities.Player
{
    [AddComponentMenu("Alphabet/Entities/Player/Player Controller")]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerController : MonoBehaviour
    {
        #region Struct
        [Serializable]
        private struct AnimationData
        {
            public string type;
            [SpineAnimation] public string[] name;
        }
        #endregion

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

        // [Header("Animation")]
        // private SkeletonMecanim _skeletonMecanim;
        // private Skeleton _playerSkeleton;
        // public Animator PlayerAnimator {get; private set;}
        
        [Header("Reference")] 
        private Rigidbody2D _playerRb;
        private PlayerAnimation _playerAnimation;
        public PlayerPickThrow PlayerPickThrow { get; private set; }
        public AnalogInputHandler PlayerInputHandler { get; private set; }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            // Component
            _playerRb = GetComponent<Rigidbody2D>();
            _playerAnimation = GetComponentInChildren<PlayerAnimation>();
            // PlayerAnimator = GetComponentInChildren<Animator>();

            // Animation
            // _skeletonMecanim = GetComponentInChildren<SkeletonMecanim>();
            // _playerSkeleton = _skeletonMecanim.skeleton;

            // Handler
            PlayerPickThrow = GetComponent<PlayerPickThrow>();
            PlayerInputHandler = GetComponentInChildren<AnalogInputHandler>();
        }

        private void Start()
        {
            InitializePlayer();
            StartMovement();
        }

        private void FixedUpdate()
        {
            PlayerMove();
        }
        
        // private void Update()
        // {
        //     PlayerAnimation();
        //     PlayerFlip();
        // }
        
        #endregion
        
        #region Methods
        
        // !-- Initialization
        private void InitializePlayer()
        {
            // Data
            playerData = PlayerDatabase.Instance.GetPlayerDatabySelected();

            // Component
            gameObject.name = playerData.PlayerName;
            CurrentMoveSpeed = playerData.PlayerMoveSpeed;
            // _playerAnimator.runtimeAnimatorController = playerData.PlayerAnimatorController;
        }
        
        // !-- Core Functionality
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

        // private void PlayerAnimation()
        // {
        //     if (movementDirection != Vector2.zero)
        //     {
        //         PlayerAnimator.SetFloat(HORIZONTAL_KEY, movementDirection.x);
        //         PlayerAnimator.SetFloat(VERTICAL_KEY, movementDirection.y);
        //         PlayerAnimator.SetBool(IS_MOVE, true);
        //     }
        //     else
        //     {
        //         PlayerAnimator.SetBool(IS_MOVE, false);
        //     }
        // }

        // private void PlayerFlip()
        // {
        //     if (!CanFlip()) return;
        //     _playerSkeleton.ScaleX *= -1;
        // }

        // private bool CanFlip()
        // {
        //     var direction = movementDirection;
        //     return direction.x < 0 && _playerSkeleton.ScaleX > 0 || direction.x > 0 && _playerSkeleton.ScaleX < 0;
        // }
        
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

            // PlayerAnimator.SetFloat(HORIZONTAL_KEY, direction.x);
            // PlayerAnimator.SetFloat(VERTICAL_KEY, direction.y);
        }
        
        #endregion
    }
}
