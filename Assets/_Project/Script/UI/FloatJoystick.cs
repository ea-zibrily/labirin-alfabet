using System;
using UnityEngine;

namespace CariHuruf.UI
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class FloatJoystick : MonoBehaviour
    {
        #region Variable

        [SerializeField] private RectTransform restrictionRectTransform;
        [field: SerializeField] public RectTransform KnobRectTransform { get; set; }
        public RectTransform JoyRectTransform { get; private set; }
        public Vector2 ScreenRestriction { get; private set; }
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            JoyRectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            InitializeJoystick();
        }

        #endregion

        #region Zimbril Callbacks

        private void InitializeJoystick()
        {
            var centerPoint = new Vector2(0.5f, 0.5f);
            
            JoyRectTransform.pivot = centerPoint;
            KnobRectTransform.anchorMin = centerPoint;
            KnobRectTransform.anchorMax = centerPoint;
            KnobRectTransform.pivot = centerPoint;
            KnobRectTransform.anchoredPosition = Vector2.zero;

            var rect = restrictionRectTransform.rect;
            ScreenRestriction = new Vector2(rect.width, rect.height);
            
            gameObject.SetActive(false);
        }

        #endregion
    }
}