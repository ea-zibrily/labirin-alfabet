using LabirinKata.UI;
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
        private FloatJoystick _floatJoystick;
        private Finger _movementFinger;

        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _floatJoystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FloatJoystick>();
        }
        
        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            ETouch.Touch.onFingerDown += TouchOnFingerDown;
            ETouch.Touch.onFingerUp += TouchOnFingerUp;
            ETouch.Touch.onFingerMove+= TouchOnFingerMove;
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
        
        // NOTE: Dipanggil waktu layar mendapat touch input
        private void TouchOnFingerDown(Finger fingerTouch)
        {
            var touchScreen = fingerTouch.screenPosition.x <= Screen.width / 2f 
                              && fingerTouch.screenPosition.y <= Screen.height / 1.2f;
            var isFullScreen = canFullScreen || touchScreen;
            
            if (_movementFinger == null && isFullScreen)
            {
                InitializeTouchOnScreen(fingerTouch);
            }
        }
        
        // NOTE: Dipanggil waktu touch input terlepas dari layar
        private void TouchOnFingerUp(Finger fingerTouch)
        {
            if (fingerTouch != _movementFinger)
            {
                return;
            }
            
            ResetTouchOn();
        }
        
        // NOTE: Dipanggil waktu touch input digerakkan pada layar
        private void TouchOnFingerMove(Finger fingerTouch)
        {
            if (fingerTouch != _movementFinger)
            {
                return;
            } 
            
            MoveTouchOn(fingerTouch);
        }
        
        #endregion
        
        #region Zimbril Callbacks

        // NOTE: Pakai method ini jika pakai ui panel restriction
        private void InitializeTouchOn(Finger fingerOn)
        {
            _movementFinger = fingerOn;
            Direction = Vector2.zero;
            _floatJoystick.gameObject.SetActive(true);
            _floatJoystick.JoyRectTransform.sizeDelta = joystickSize;
            _floatJoystick.JoyRectTransform.anchoredPosition = fingerOn.screenPosition;
        }

        // NOTE: Pakai method ini jika pakai screen restriction
        private void InitializeTouchOnScreen(Finger fingerOn)
        {
            _movementFinger = fingerOn;
            Direction = Vector2.zero;
            _floatJoystick.gameObject.SetActive(true);
            _floatJoystick.JoyRectTransform.sizeDelta = joystickSize;
            _floatJoystick.JoyRectTransform.anchoredPosition = ScreenClampStartPosition(fingerOn.screenPosition);
        }

        private void MoveTouchOn(Finger fingerMove)
        {
            Vector2 knobPosition;
            var maxMovement = joystickSize.x / 2f;
            var currentTouch = fingerMove.currentTouch;
            
            if (Vector2.Distance(currentTouch.screenPosition, _floatJoystick.JoyRectTransform.anchoredPosition) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - _floatJoystick.JoyRectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - _floatJoystick.JoyRectTransform.anchoredPosition;
            }
            
            _floatJoystick.KnobRectTransform.anchoredPosition = knobPosition;
            Direction = knobPosition / maxMovement;
        }
        
        private void ResetTouchOn()
        {
            _movementFinger = null;
            _floatJoystick.KnobRectTransform.anchoredPosition = Vector2.zero;
            _floatJoystick.gameObject.SetActive(false);
            Direction = Vector2.zero;
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