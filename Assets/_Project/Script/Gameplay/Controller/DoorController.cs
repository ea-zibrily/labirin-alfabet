using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using LabirinKata.Stage;
using LabirinKata.Entities.Player;
using LabirinKata.Gameplay.EventHandler;

namespace LabirinKata.Gameplay.Controller
{
    public class DoorController : MonoBehaviour
    {
        #region Constant Variable
        
        private const string OPEN_DOOR_TRIGGER = "Open";
        private const string MOVE_CAMERA_TRIGGRER = "IsMove";

        #endregion
        
        #region Variable

        [Header("Door")] 
        [SerializeField] private float doorOpenDelay;
        [SerializeField] private float reachDoorDelay;
        
        [Header("Camera")]
        [SerializeField] private float cameraMoveInDelay;
        [SerializeField] private float cameraMoveOutDelay;
        [SerializeField] private CinemachineVirtualCamera doorVirtualCamera;
        [SerializeField] private Animator doorCameraAnimator;
        
        //-- Camera Event
        public static event Action OnCameraShiftIn;
        public static event Action OnCameraShiftOut;

        [Header("Reference")] 
        private BoxCollider2D _boxCollider2D;
        private Animator _doorAnimator;
        private DoorEventHandler _doorEventHandler;
        private PlayerController _playerController;
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _doorAnimator = GetComponentInChildren<Animator>();
            _doorEventHandler = GetComponentInChildren<DoorEventHandler>();
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        
        private void OnEnable()
        {
            GameEventHandler.OnObjectiveClear += OpenDoor;
            _doorEventHandler.OnDoorOpen += ActivateDoorTrigger;
        }
        
        private void OnDisable()
        {
            GameEventHandler.OnObjectiveClear -= OpenDoor;
            _doorEventHandler.OnDoorOpen -= ActivateDoorTrigger;
        }

        private void Start()
        {
            InitializeDoor();
        }

        #endregion
        
        #region CariHuruf Callbacks
        
        // !-- Initialization
        private void InitializeDoor()
        {
            doorVirtualCamera.Follow = gameObject.transform;
            _boxCollider2D.isTrigger = false;
        }
        
        // !-- Core Functionality
        private void OpenDoor() => StartCoroutine(OpenDoorRoutine());
        private void ActivateDoorTrigger() => StartCoroutine(ActivateDoorTriggerRoutine());
        
        private IEnumerator OpenDoorRoutine()
        {
            yield return new WaitForSeconds(cameraMoveInDelay);
            doorCameraAnimator.SetBool(MOVE_CAMERA_TRIGGRER, true);
            _playerController.SetPlayerDirection(gameObject.transform);
            OnCameraShiftIn?.Invoke();
            
            yield return new WaitForSeconds(doorOpenDelay);
            _doorAnimator.SetTrigger(OPEN_DOOR_TRIGGER);
        }
        
        private IEnumerator ActivateDoorTriggerRoutine()
        {
            yield return new WaitForSeconds(cameraMoveOutDelay);
            doorCameraAnimator.SetBool(MOVE_CAMERA_TRIGGRER, false);
            OnCameraShiftOut?.Invoke();
            
            _boxCollider2D.isTrigger = true;
            doorVirtualCamera.Follow = null;
        }
        
        #endregion
        
        #region Collider Callbacks
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) return;

            if (StageManager.Instance.CheckCanContinueStage())
            {
                GameEventHandler.ContinueStageEvent();
            }
            else
            { 
                GameEventHandler.GameWinEvent();
            }
        }
        
        #endregion
    }
}