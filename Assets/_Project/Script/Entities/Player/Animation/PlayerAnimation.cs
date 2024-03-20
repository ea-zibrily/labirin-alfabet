using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;
using UnityEngine.UIElements;

namespace Alphabet.Entities.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        #region Fields

        [SpineAnimation] private string _currentState;

        private bool _isRight;

        private SkeletonAnimation _skeletonAnimation;
        private Spine.AnimationState _playerAnimationState;
        private Spine.Skeleton _playerSkeleton;

        // Reference
        private PlayerController _playerController;

        #endregion

        #region Cached Properties

        // Side
        private readonly string Side_Idle = "QF_idle";
        private readonly string Side_IdleHold = "QF_idle+holding";
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

            _skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
            _playerAnimationState = _skeletonAnimation.state;
            _playerSkeleton = _skeletonAnimation.Skeleton;
       }

       private void Start()
       {
            InitializePlayerAnimation();
       }

        private void Update()
        {
            AnimationHandler();
        }

        #endregion

        #region Methods

        // !-- Initialization
        private void InitializePlayerAnimation()
        {
            _isRight = true;
            _currentState = Side_Idle;
            _playerAnimationState.SetAnimation(0, _currentState, true);
        }
        
        // !-- Core Functionality
        private void AnimationHandler()
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
            if (_playerController.MovementDirection == Vector2.zero)
            {
                return _currentState switch
                {
                    "QF_walk" => Side_Idle,
                    "F_walk" => Front_Idle,
                    "B_walk" => Back_Idle,
                    _ => _currentState,
                };
            }

            if (_playerController.MovementDirection.x != 0) return Side_Walk;
            if (_playerController.MovementDirection.y > 0) return Front_Walk;
            if (_playerController.MovementDirection.y < 0) return Back_Walk;
            
            return _currentState;
        }

        private void ChangeAnimation(string state)
        {
            var isLooping = ShouldAnimationLoop(state);
            Debug.Log(isLooping);
            _playerAnimationState.SetAnimation(0, state, isLooping);
        }

        private void PlayerFlip()
        {
            _isRight = !_isRight;
            _playerSkeleton.ScaleX *= -1;
            Debug.Log("go flip");
        }

        // !-- Helper/Utilities
        private bool ShouldAnimationLoop(string state)
        {
            return state != Side_Shoot || state != Front_Shoot || state != Back_Shoot;
        }

        #endregion

    }
}
