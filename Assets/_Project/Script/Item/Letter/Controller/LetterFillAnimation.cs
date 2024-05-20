using System;
using System.Collections;
using UnityEngine;

namespace Alphabet.Item
{
    public class LetterFillAnimation : MonoBehaviour
    {
        #region Struct
        [Serializable]
        public struct LetterScale
        {
            public Vector2 Default;
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



        #endregion

        #region Methods

        // !-- Initialization
        private IEnumerator InitializeCenterPosition(RectTransform rectTarget)
        {
            var centerPosition = Vector3.zero;
            var i = 0;

            while (i < 3)
            {
                yield return null;
                centerPosition = SwitchToRectTransform(letterPoint, rectTarget);
                i++;
            }
            rectTarget.anchoredPosition = centerPosition;
        }

        // !-- Core Functionality
        public IEnumerator AnimateLetterRoutine(GameObject letterTarget)
        {
            // Initialize
            var letterRect = letterTarget.GetComponent<RectTransform>();

            yield return InitializeCenterPosition(letterRect);
            _targetPosition = Vector2.zero;
            letterRect.localScale = Vector3.zero;

            // Animate
            Debug.Log("Go Animate!");
            var lerpStartDelay = tweeningDuration * 2 + 1f;
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

            Debug.Log("done animate!");
            target.localPosition = point;
            target.localScale = letterScale.Default;
        }

        #endregion
    }
}