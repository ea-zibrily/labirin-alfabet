﻿using LabirinKata.UI;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace LabirinKata.Entities.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Variable
        
        [Header("Joystick Settings")] 
        [Tooltip("Isi dengan ukuran Rect Transform joystick yang diinginkan")]
        [SerializeField] private Vector2 joystickSize;
        [Tooltip("Aktifkan jika ingin mengakses joystick dalam full screen")]
        [SerializeField] private bool canFullScreen;
        
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
            var touchScreen = fingerTouch.screenPosition.x <= Screen.width / 2f 
                              && fingerTouch.screenPosition.y <= Screen.height / 1.4f;
            var isFullScreen = canFullScreen || touchScreen;
            
            if (_movementFinger == null && isFullScreen)
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
        
        #endregion
        
        #region Labirin Kata Callbacks

        //-- Initialization
        private void InitializeTouchOnUI(Finger fingerOn)
        {
            _movementFinger = fingerOn;
            Direction = Vector2.zero;
            _floatingJoystickHandler.gameObject.SetActive(true);
            _floatingJoystickHandler.JoyRectTransform.sizeDelta = joystickSize;
            _floatingJoystickHandler.JoyRectTransform.anchoredPosition = fingerOn.screenPosition;
        }

        private void InitializeTouchOnScreen(Finger fingerOn)
        {
            _movementFinger = fingerOn;
            Direction = Vector2.zero;
            _floatingJoystickHandler.gameObject.SetActive(true);
            _floatingJoystickHandler.JoyRectTransform.sizeDelta = joystickSize;
            _floatingJoystickHandler.JoyRectTransform.anchoredPosition = ScreenClampStartPosition(fingerOn.screenPosition);
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

        private Vector2 ScreenClampStartPosition(Vector2 startPosition)
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