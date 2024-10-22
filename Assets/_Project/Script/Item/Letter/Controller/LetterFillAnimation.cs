using System;
using System.Collections;
using UnityEngine;
using Alphabet.Enum;
using Alphabet.Managers;

namespace Alphabet.Letter
{
    public class LetterFillAnimation : MonoBehaviour
    {
        #region Struct
        [Serializable]
        public struct LetterScale
        {
            public Vector3 Default;
            public Vector2 Upper;
            public Vector2 OnMove;
        }
        #endregion
        
        #region Fields & Properties
        
        [Header("UI")]
        [SerializeField] private RectTransform letterPoint;
        [SerializeField] private LetterScale letterScale;        

        private Vector2 _targetPosition;

        [Header("Animation")]
        [SerializeField] private LeanTweenType leanTweenType;
        [SerializeField] private float tweeningDuration;
        [SerializeField] private float moveDelay;
        [SerializeField] private float lerpDuration;

        private readonly float tweenDelayMultiplier = 0.5f;
        public float LerpDuration => lerpDuration;

        // Utilities
        public GameObject TargetFill { get; private set; }
        public bool IsAnimate { get; private set; }

        #endregion
        
        #region Methods

        // !-- Initialization
        private void InitializeCenterPosition(RectTransform rectTarget)
        {
            var centerPosition = SwitchToRectTransform(letterPoint, rectTarget);
            rectTarget.anchoredPosition = centerPosition;
        }

        // !-- Core Functionality
        public IEnumerator AnimateLetterRoutine(GameObject letterTarget)
        {
            // Initialize
            var letterRect = letterTarget.GetComponent<RectTransform>();

            InitializeCenterPosition(letterRect);
            _targetPosition = Vector2.zero;
            letterRect.localScale = Vector3.zero;
            TargetFill = letterTarget;
            letterTarget.SetActive(true);

            // Animate
            IsAnimate = true;
            var lerpStartDelay = tweeningDuration * 2 + tweenDelayMultiplier;
            LeanTween.scale(letterRect, letterScale.Upper, tweeningDuration).setEase(leanTweenType);
            LeanTween.scale(letterRect, letterScale.OnMove, tweeningDuration)
                .setDelay(tweeningDuration).setEase(leanTweenType);

            // Move
            yield return new WaitForSeconds(lerpStartDelay);
            yield return LerpToTargetRoutine(_targetPosition, letterRect);
        }

        private Vector2 SwitchToRectTransform(RectTransform from, RectTransform to)
        {
            Vector2 fromPivotDerivedOffset = new(
                from.rect.width + from.rect.xMin,
                from.rect.height + from.rect.yMin);

            Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
            screenP += fromPivotDerivedOffset;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out Vector2 localPoint);
            Vector2 pivotDerivedOffset = new(to.rect.width + to.rect.xMin, to.rect.height + to.rect.yMin);
            return to.anchoredPosition + localPoint - pivotDerivedOffset;
        }

        private IEnumerator LerpToTargetRoutine(Vector3 point, RectTransform target)
        {
            var elapsedTime = 0f;
            while (elapsedTime < moveDelay)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
                var lerpRatio = elapsedTime / lerpDuration;
                target.localPosition = Vector3.Lerp(target.anchoredPosition, point, lerpRatio);
                target.localScale = Vector3.Lerp(target.localScale, letterScale.Default, lerpRatio);
            }

            FindObjectOfType<AudioManager>().PlayAudio(Musics.LetterUISfx);
            target.localPosition = point;
            target.localScale = letterScale.Default;
            IsAnimate = false;
            TargetFill = null;
        }

        #endregion
    }
}