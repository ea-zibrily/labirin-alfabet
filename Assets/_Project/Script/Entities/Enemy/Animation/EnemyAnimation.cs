using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

namespace Alphabet.Entities.Enemy
{
    public class EnemyAnimation : MonoBehaviour
    {
        #region Fields & Properties

        // Component
        private bool _isRight;

        // Animation
        [SpineAnimation] private string _currentState;

        private SkeletonAnimation _skeletonAnimation;
        private Spine.AnimationState _enemyAnimationState;
        private Skeleton _enemySkeleton;

        // Reference
        private EnemyBase _enemyController;
        private EnemyManager _enemyManager;

        #endregion

        #region Cached Properties
        // Side
        private readonly string Side_Idle = "QF_idle";
        private readonly string Side_Walk = "QF_walk";

        // Front
        private readonly string Front_Idle = "F_idle";
        private readonly string Front_Walk = "F_walk";

        // Back
        private readonly string Back_Idle = "B_idle";
        private readonly string Back_Walk = "B_walk";

        // Stunned
        private readonly string STUNNED = "stunned";

        #endregion


        #region MonoBehavior Callbacks

        private void Awake()
        {
            var enemy = transform.parent.gameObject;
            _enemyController = enemy.GetComponent<EnemyBase>();
            _enemyManager = enemy.GetComponent<EnemyManager>();

            _skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            _enemyAnimationState = _skeletonAnimation.state;
            _enemySkeleton = _skeletonAnimation.skeleton;

            InitializeAnimation();
        }

        private void Update()
        {
            AnimationHandler();
        }

        #endregion

        #region Methods

        // !-- Initialization
        private void InitializeAnimation()
        {
            _isRight = true;
            // _currentState = Side_Idle;
            _currentState = Side_Walk;
            if (_enemyAnimationState == null)
            {
                Debug.LogWarning("Player Animation State is null");
                return;
            }
            _enemyAnimationState.SetAnimation(0, _currentState, true);
        }

        // !-- Core Functionality
        public void AnimationHandler()
        {
            var direction = _enemyController.MovementDirection;
            if ((direction.x > 0 && !_isRight) || (direction.x < 0 && _isRight)) PlayerFlip();

            var state = GetState();

            if (state == _currentState) return;
            ChangeAnimation(state);
            _currentState = state;
        }

        private string GetState()
        {
            // Movement direction
            var movementDirection = _enemyController.MovementDirection;
            var movingHorizontally = movementDirection.x != 0;
            var movingVertically = movementDirection.y != 0;
            var isStunned = _enemyManager.IsStunned;

            // Handle normal state
            if (isStunned) return STUNNED;

            if (movingHorizontally) return Side_Walk;
            if (movingVertically) return movementDirection.y > 0 ? Back_Walk : Front_Walk;

            // TODO: Uncomment logic dibawah klo jadi pake Idle State
            // if (movementDirection == Vector2.zero)
            // {
            //     return _currentState switch
            //     {
            //         "QF_walk" => Side_Idle,
            //         "F_walk" => Front_Idle,
            //         "B_walk" => Back_Idle,
            //         _ => _currentState,
            //     };
            // }
            
            return _currentState;
        }

        private void ChangeAnimation(string state)
        {
           var stateName = state;
            _enemyAnimationState.SetAnimation(0, stateName, true);
        }

        private void PlayerFlip()
        {
            _isRight = !_isRight;
            _enemySkeleton.ScaleX *= -1;
        }

        #endregion

        
    }
}
