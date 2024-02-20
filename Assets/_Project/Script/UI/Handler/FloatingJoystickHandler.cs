using UnityEngine;

namespace LabirinKata.UI
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class FloatingJoystickHandler : MonoBehaviour
    {
        #region Variable
        
        [field: SerializeField] public RectTransform KnobRectTransform { get; set; }
        public RectTransform JoyRectTransform { get; private set; }
        
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

        #region Labirin Kata Callbacks

        private void InitializeJoystick()
        {
            var centerPoint = new Vector2(0.5f, 0.5f);
            
            JoyRectTransform.pivot = centerPoint;
            KnobRectTransform.anchorMin = centerPoint;
            KnobRectTransform.anchorMax = centerPoint;
            KnobRectTransform.pivot = centerPoint;
            KnobRectTransform.anchoredPosition = Vector2.zero;
        }

        #endregion
    }
}