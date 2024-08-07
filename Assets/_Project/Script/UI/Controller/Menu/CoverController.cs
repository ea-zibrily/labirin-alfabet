using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

namespace Alphabet.UI
{
    public class CoverController : MonoBehaviour
    {
        #region Fields & Property

        // Reference
        private SkeletonAnimation _skeletonAnimation;
        private Spine.AnimationState _coverAnimationState;
        private float _startDuration;
        private bool _isFirstAnimate = true;

        // Const Variable
        private const string START_ANIMATION = "start";
        private const string LOOP_ANIMATION = "loop";

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _skeletonAnimation = GetComponent<SkeletonAnimation>();
        }

        private void OnEnable()
        {
            if (_isFirstAnimate) InitializeCover();
            StartCoroutine(PlayAnimationRoutine());
        }

        #endregion

        #region Methods

        // !-- Initialization
        private void InitializeCover()
        {
            _coverAnimationState = _skeletonAnimation.AnimationState;
            var startAnimate = _skeletonAnimation.Skeleton.Data.FindAnimation(START_ANIMATION);
            _startDuration = startAnimate.Duration;
            _isFirstAnimate = false;
        }
        
        // !-- Core Functionality
        private IEnumerator PlayAnimationRoutine()
        {
            _coverAnimationState.SetAnimation(0, START_ANIMATION, false);
            yield return new WaitForSeconds(_startDuration);

            _coverAnimationState.SetAnimation(0, LOOP_ANIMATION, true);
        }

        #endregion
    }
}
