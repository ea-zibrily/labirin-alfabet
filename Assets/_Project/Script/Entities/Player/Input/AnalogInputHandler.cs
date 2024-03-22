using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Alphabet.UI;

using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace Alphabet.Entities.Player
{
    [AddComponentMenu("Alphabet/Entities/Player/Input/Analog Input Handler")]
    public class AnalogInputHandler : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Settings")] 
        [Tooltip("Isi dengan ukuran Rect Transform joystick yang diinginkan")]
        [SerializeField] private Vector2 joystickSize;
        
        [Tooltip("Aktifkan jika ingin mengakses joystick dalam half screen")]
        [SerializeField] private bool isHalfScreen;

        [Tooltip("Aktifkan jika ingin memberi batasan tepi layar joystick")]
        [SerializeField] private bool isClampPosition;
        
        public Vector2 Direction { get; private set; }
        
        [Header("Reference")] 
        private GameObject _joystickObjectUI;
        private FloatingJoystickHandler _floatingJoystickHandler;
        private Finger _movementFinger;
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _joystickObjectUI = GameObject.FindGameObjectWithTag("Joystick");
            _floatingJoystickHandler = _joystickObjectUI.GetComponent<FloatingJoystickHandler>();
        }
        
        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            ETouch.Touch.onFingerDown += TouchOnFingerDown;
            ETouch.Touch.onFingerUp += TouchOnFingerUp;
            ETouch.Touch.onFingerMove += TouchOnFingerMove;
        }
        
        private void OnDisable()
        {
            ETouch.Touch.onFingerDown -= TouchOnFingerDown;
            ETouch.Touch.onFingerUp -= TouchOnFingerUp;
            ETouch.Touch.onFingerMove -= TouchOnFingerMove;
            EnhancedTouchSupport.Disable();
        }
        
        #endregion
        
        #region Enhanced Touch Callbacks
        
        // !-- Core Functionality
        private void TouchOnFingerDown(Finger fingerTouch)
        {
            if (_movementFinger == null && IsTouchWithinRestrictedArea(fingerTouch))
            {
                InitializeTouchOnScreen(fingerTouch);
            }
        }
        
        private void TouchOnFingerUp(Finger fingerTouch)
        {
            if (fingerTouch != _movementFinger)
            {
                return;
            }
            
            ResetTouchOn();
        }
        
        private void TouchOnFingerMove(Finger fingerTouch)
        {
            if (fingerTouch != _movementFinger)
            {
                return;
            } 
            
            MoveTouchOn(fingerTouch);
        }
        
        // !-- Helper/Utilities
        private bool IsTouchWithinRestrictedArea(Finger fingerTouch)
        {
            if (isHalfScreen)
            {
                return fingerTouch.screenPosition.x <= Screen.width / 2f 
                       && fingerTouch.screenPosition.y <= Screen.height / 1.3f;
            }
            
            return fingerTouch.screenPosition.y <= Screen.height / 1.3f 
                    && fingerTouch.screenPosition.y >= Screen.height - (Screen.height / 1.25f);
        }
        
        #endregion
        
        #region Labirin Kata Callbacks

        // !-- Initialization
        private void InitializeTouchOnScreen(Finger fingerOn)
        {
            _movementFinger = fingerOn;
            Direction = Vector2.zero;
            _joystickObjectUI.SetActive(true);
            _floatingJoystickHandler.JoyRectTransform.sizeDelta = joystickSize;

            var screenStartPosition = isClampPosition ? ClampStartPosition(fingerOn.screenPosition) : fingerOn.screenPosition;
            _floatingJoystickHandler.JoyRectTransform.anchoredPosition = screenStartPosition;
        }
        
        // !-- Core Functionality
        private void MoveTouchOn(Finger fingerMove)
        {
            Vector2 knobPosition;
            var maxMovement = joystickSize.x / 2f;
            var currentTouch = fingerMove.currentTouch;
            
            if (Vector2.Distance(currentTouch.screenPosition, _floatingJoystickHandler.JoyRectTransform.anchoredPosition) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - _floatingJoystickHandler.JoyRectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - _floatingJoystickHandler.JoyRectTransform.anchoredPosition;
            }
            
            _floatingJoystickHandler.KnobRectTransform.anchoredPosition = knobPosition;
            Direction = knobPosition / maxMovement;
        }
        
        private void ResetTouchOn()
        {
            _movementFinger = null;
            _floatingJoystickHandler.KnobRectTransform.anchoredPosition = Vector2.zero;
            _joystickObjectUI.SetActive(false);
            Direction = Vector2.zero;
        }
        
        // !-- Helper/Utilities
        public void EnableTouchInput()
        {
            enabled = true;
        }
        
        public void DisableTouchInput()
        {
            ResetTouchOn();
            enabled = false;
        }

        private Vector2 ClampStartPosition(Vector2 startPosition)
        {
            if (startPosition.x < joystickSize.x / 2)
            {
                startPosition.x = joystickSize.x / 2;
            }
            else if (startPosition.x > Screen.width - joystickSize.x / 2)
            {
                startPosition.x = Screen.width - joystickSize.x / 2;
            }

            if (startPosition.y < joystickSize.y / 2)
            {
                startPosition.y = joystickSize.y / 2;
            }
            else if (startPosition.y > Screen.height - joystickSize.y / 2)
            {
                startPosition.y = Screen.height - joystickSize.y / 2;
            }
            
            return startPosition;
        }
        
        #endregion
    }
}