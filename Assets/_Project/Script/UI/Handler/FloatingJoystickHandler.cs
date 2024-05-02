using UnityEngine;

namespace Alphabet.UI
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class FloatingJoystickHandler : MonoBehaviour
    {
        #region Fields and Properties
        
        [Header("UI")]
        [SerializeField] private RectTransform knobRectTransform;

        public RectTransform KnobRect
        {
            get => knobRectTransform;
            set => knobRectTransform = value;
        }
        public RectTransform JoystickRect { get; private set; }
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            JoystickRect = GetComponent<RectTransform>();
        }
        
        private void Start()
        {
            var centerPoint = new Vector2(0.5f, 0.5f);
            
            JoystickRect.pivot = centerPoint;
            KnobRect.anchorMin = centerPoint;
            KnobRect.anchorMax = centerPoint;
            KnobRect.pivot = centerPoint;
            KnobRect.anchoredPosition = Vector2.zero;

            gameObject.SetActive(false);
        }
        
        #endregion
    }
}