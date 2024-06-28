using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Alphabet.Enum;

namespace Alphabet.Entities.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        #region Fields
        
        [Header("Component")]
        [SerializeField, Range(0.5f, 2f)] private float speedUpTimescale;
        [SerializeField, Range(0.5f, 1.5f)] private float holdSpeedUpTimescale;
        [SerializeField] private bool _isRight;

        // Const Variable
        private const float DEFAULT_TIMESCALE = 1f;

        // Animation
        [SpineAnimation] private string _currentState;

        private SkeletonAnimation _skeletonAnimation;
        private Spine.AnimationState _playerAnimationState;
        private Skeleton _playerSkeleton;

        // Reference
        private PlayerController _playerController;
        private PlayerManager _playerManager;

        #endregion

        #region Cached Properties
        // Side
        private readonly string Side_Idle = "QF_idle";
        private readonly string Side_IdleHold = "QF_idle_holding";
        private readonly string Side_Walk = "QF_walk";
        private readonly string Side_WalkHold = "QF_walk+holding";
        private readonly string Side_Shoot = "QF_shot";

        // Front
        private readonly string Front_Idle = "F_idle";
        private readonly string Front_IdleHold = "F_idle+holding";
        private readonly string Front_Walk = "F_walk";
        private readonly string Front_WalkHold = "F_walk+holding";
        private readonly string Front_Shoot = "F_shot";

        // Front
        private readonly string Back_Idle = "B_idle";
        private readonly string Back_IdleHold = "B_idle+holding";
        private readonly string Back_Walk = "B_walk";
        private readonly string Back_WalkHold = "B_walk+holding";
        private readonly string Back_Shoot = "B_shot";
        #endregion

        #region MonoBehaviour Callbacks

       private void Awake()
       {
            _playerController = transform.parent.GetComponent<PlayerController>();
            _playerManager = _playerController.GetComponentInChildren<PlayerManager>();
            _skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
       }

        private void Start()
        {
            _playerAnimationState = _skeletonAnimation.state;
            _playerSkeleton = _skeletonAnimation.Skeleton;

            InitializePlayerAnimation();
        }

        private void Update()
        {
            var isThrowingItem = _playerController.PlayerPickThrow.IsThrowItem;
            if (isThrowingItem) return;
            AnimationHandler();
        }

        #endregion

        #region Methods

        // !-- Initialization
        private void InitializePlayerAnimation()
        {
            _isRight = true;
            _currentState = Side_Idle;
            if (_playerAnimationState == null)
            {
                Debug.LogWarning("Player Animation State is null");
                return;
            }
            _playerAnimationState.SetAnimation(0, _currentState, true);
        }
        
        // !-- Core Functionality
        public void AnimationHandler()
        {
            var direction = _playerController.MovementDirection;
            if ((direction.x > 0 && !_isRight) || (direction.x < 0 && _isRight)) PlayerFlip();

            var state = GetState();

            if (state == _currentState) return;
            ChangeAnimation(state);
            _currentState = state;
        }

        private string GetState()
        {
            // Cache properties
            var isHoldingItem = _playerController.PlayerPickThrow.IsHoldedItem;

            // Movement direction
            var movementDirection = _playerController.MovementDirection;
            var movingHorizontally = movementDirection.x != 0;
            var movingVertically = movementDirection.y != 0;

            if (isHoldingItem)
            {
                // Handle idle and walking states
                if (movementDirection == Vector2.zero)
                {
                    return _currentState switch
                    {
                        "QF_walk+holding" or "QF_walk" or "QF_idle" => Side_IdleHold,
                        "F_walk+holding" or "F_walk" or "F_idle" => Front_IdleHold,
                        "B_walk+holding" or "B_walk" or "B_idle" => Back_IdleHold,
                        _ => _currentState,
                    };
                }

                if (movingHorizontally) return Side_WalkHold;
                if (movingVertically) return movementDirection.y > 0 ? Back_WalkHold : Front_WalkHold;
            }
            else
            {
                // Handle normal state
                if (movementDirection == Vector2.zero)
                {
                    return _currentState switch
                    {
                        "QF_walk" => Side_Idle,
                        "F_walk" => Front_Idle,
                        "B_walk" => Back_Idle,
                        _ => _currentState,
                    };
                }

                if (movingHorizontally) return Side_Walk;
                if (movingVertically) return movementDirection.y > 0 ? Back_Walk : Front_Walk;
            }

            return _currentState;
        }

        private string GetThrowState()
        {
             // Movement direction
            var movementDirection = _playerController.MovementDirection;
            var movingHorizontally = movementDirection.x != 0;
            var movingVertically = movementDirection.y != 0;

            // Handle shoot state
            if (movementDirection == Vector2.zero)
                {
                    return _currentState switch
                    {
                        "QF_walk+holding" or "QF_idle_holding" => Side_Shoot,
                        "F_walk+holding" or "F_idle+holding" => Front_Shoot,
                        "B_walk+holding" or "B_idle+holding" => Back_Shoot,
                        _ => _currentState,
                    };
                }

            if (movingHorizontally) return Side_Shoot;
            if (movingVertically) return movementDirection.y > 0 ? Back_Shoot : Front_Shoot;

            return _currentState;
        }

        public void CallThrowState()
        {
            var throwState = GetThrowState();
            _currentState = throwState;
            _playerAnimationState.SetAnimation(0, throwState, false);
        }

        private void ChangeAnimation(string state)
        {
            var isLooping = ShouldAnimationLoop(state);
            var buffedTimeScale = _playerController.PlayerPickThrow.IsHoldedItem ? holdSpeedUpTimescale : speedUpTimescale;
            // var timeScale = _playerController.IsBuffed ? buffedTimeScale : DEFAULT_TIMESCALE;
            var timeScale = _playerManager.HasBuffEffect[BuffType.Speed] ? buffedTimeScale : DEFAULT_TIMESCALE;
            
            _playerAnimationState.SetAnimation(0, state, isLooping).TimeScale = timeScale;
        }
        
        private void PlayerFlip()
        {
            _isRight = !_isRight;
            _playerSkeleton.ScaleX *= -1;
        }

        // !-- Helper/Utilities
        public void SetInitializeSkin(string skin)
        {
            _skeletonAnimation.initialSkinName = skin;
        }

        public void ChangeSkinWhenPlay(string skin)
        {
            _skeletonAnimation.Skeleton.SetSkin(skin);
            _skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            _skeletonAnimation.LateUpdate();
        }

        public void SetDefaultState()
        {
            var direction = _playerController.MovementDirection;

            if ((direction.x > 0 && !_isRight) || (direction.x < 0 && _isRight)) PlayerFlip();
            ChangeAnimation(Side_Idle);
        }

        private bool ShouldAnimationLoop(string state)
        {
            return state != Side_Shoot || state != Front_Shoot || state != Back_Shoot;
        }

        #endregion

    }
}
