using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Alphabet.UI;

using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace Alphabet.Entities.Player
{
    public class JoystickInputHandler : MonoBehaviour
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
        [SerializeField] private FloatingJoystickHandler _joystickHandler;
        private Finger _movementFinger;
        private PlayerController _playerController;
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _playerController = transform.parent.gameObject.GetComponent<PlayerController>();
        }
        
        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            ETouch.Touch.onFingerDown += OnFingerDown;
            ETouch.Touch.onFingerUp += OnFingerUp;
            ETouch.Touch.onFingerMove += OnFingerMove;
        }
        
        private void OnDisable()
        {
            ETouch.Touch.onFingerDown -= OnFingerDown;
            ETouch.Touch.onFingerUp -= OnFingerUp;
            ETouch.Touch.onFingerMove -= OnFingerMove;
            EnhancedTouchSupport.Disable();
        }
        
        #endregion
        
        #region Methodss

        // !-- Core Functionality
        private void OnFingerDown(Finger fingerTouch)
        {
            if (_movementFinger == null && IsRestrictedArea(fingerTouch))
            {
                if (!_playerController.CanMove) return;
            
                _movementFinger = fingerTouch;
                Direction = Vector2.zero;
                _joystickHandler.gameObject.SetActive(true);
                _joystickHandler.JoystickRect.sizeDelta = joystickSize;

                var screenStartPosition = isClampPosition ? 
                        ClampStartPosition(fingerTouch.screenPosition) : 
                        fingerTouch.screenPosition;
                _joystickHandler.JoystickRect.anchoredPosition = screenStartPosition;
            }
        }
        
        private void OnFingerUp(Finger fingerTouch)
        {
            if (fingerTouch != _movementFinger) return;

            _movementFinger = null;
            _joystickHandler.KnobRect.anchoredPosition = Vector2.zero;
            _joystickHandler.gameObject.SetActive(false);
            Direction = Vector2.zero;
        }
        
        private void OnFingerMove(Finger fingerTouch)
        {
            if (fingerTouch != _movementFinger) return;
            
            Vector2 knobPosition;
            var maxMovement = joystickSize.x / 2f;
            var currentTouch = fingerTouch.currentTouch;
            var joystickAnchor = _joystickHandler.JoystickRect.anchoredPosition;
            
            if (Vector2.Distance(currentTouch.screenPosition, joystickAnchor) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - joystickAnchor).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - joystickAnchor;
            }
            
            _joystickHandler.KnobRect.anchoredPosition = knobPosition;
            Direction = knobPosition / maxMovement;
        }
        
        private void RemoveFinger()
        {
             _movementFinger = null;
            _joystickHandler.KnobRect.anchoredPosition = Vector2.zero;
            _joystickHandler.gameObject.SetActive(false);
            Direction = Vector2.zero;
        }

        // !-- Helper/Utilities
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

        private bool IsRestrictedArea(Finger finger)
        {
            if (isHalfScreen)
            {
                return finger.screenPosition.x <= Screen.width / 2f 
                       && finger.screenPosition.y <= Screen.height / 1.3f;
            }
            
            return finger.screenPosition.y <= Screen.height / 1.3f 
                    && finger.screenPosition.y >= Screen.height - (Screen.height / 1.25f);
        }
        
        // !-- Helper/Utilities
        public void EnableTouchInput()
        {
            enabled = true;
        }
        
        public void DisableTouchInput()
        {
            RemoveFinger();
            enabled = false;
        }
        
        #endregion
    }
}