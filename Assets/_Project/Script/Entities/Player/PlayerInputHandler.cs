using LabirinKata.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem.EnhancedTouch;

using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace LabirinKata.Entities.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Joystick Settings")] 
        [Tooltip("Isi dengan ukuran Rect Transform joystick yang diinginkan")]
        [SerializeField] private Vector2 joystickSize;
        [FormerlySerializedAs("canFullScreen")]
        [Tooltip("Aktifkan jika ingin mengakses joystick dalam half screen")]
        [SerializeField] private bool isHalfScreen;
        
        public Vector2 Direction { get; private set; }
        
        [Header("Reference")] 
        private FloatingJoystickHandler _floatingJoystickHandler;
        private Finger _movementFinger;

        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _floatingJoystickHandler = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FloatingJoystickHandler>();
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
        
        //-- Core Functionality
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
        
        //-- Helper/Utilities
        private bool IsTouchWithinRestrictedArea(Finger fingerTouch)
        {
            if (isHalfScreen)
            {
                return fingerTouch.screenPosition.x <= Screen.width / 2f 
                       && fingerTouch.screenPosition.y <= Screen.height / 1.3f;
            }
            
            return fingerTouch.screenPosition.y <= Screen.height / 1.3f;
        }
        
        #endregion
        
        #region Labirin Kata Callbacks

        //-- Initialization
        private void InitializeTouchOnScreen(Finger fingerOn)
        {
            _movementFinger = fingerOn;
            Direction = Vector2.zero;
            _floatingJoystickHandler.gameObject.SetActive(true);
            _floatingJoystickHandler.JoyRectTransform.sizeDelta = joystickSize;
            _floatingJoystickHandler.JoyRectTransform.anchoredPosition = ClampStartPosition(fingerOn.screenPosition);
        }
        
        //-- Core Functionality
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
            _floatingJoystickHandler.gameObject.SetActive(false);
            Direction = Vector2.zero;
        }
        
        //-- Helper/Utilities
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