using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using Alphabet.Stage;
using Alphabet.Entities.Player;
using Alphabet.Gameplay.EventHandler;

namespace Alphabet.Gameplay.Controller
{
    public class DoorController : MonoBehaviour
    {
        #region Struct
        [Serializable]
        public struct DoorConstraint
        {
            public PolygonCollider2D OpenConstraint;
            public PolygonCollider2D CloseConstraint;
        }
        #endregion

        #region Fields & Properties

        [Header("Door")] 
        [SerializeField] private float doorOpenDelay;
        [SerializeField] private float reachDoorDelay;
        [SerializeField] private DoorConstraint doorConstraint;
        
        [Header("Camera")]
        [SerializeField] private float cameraMoveInDelay;
        [SerializeField] private float cameraMoveOutDelay;
        [SerializeField] private CinemachineVirtualCamera doorVirtualCamera;
        [SerializeField] private Animator doorCameraAnimator;

        // Const Variable
        private const string OPEN_DOOR_TRIGGER = "Open";
        private const string MOVE_CAMERA_TRIGGRER = "IsMove";

        [Header("Reference")] 
        private CapsuleCollider2D _capsuleCollider2D;
        private Animator _doorAnimator;
        private DoorEventHandler _doorEventHandler;
        private PlayerController _playerController;
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
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
        
        #region Methods
        
        // !-- Initialization
        private void InitializeDoor()
        {
            doorVirtualCamera.Follow = gameObject.transform;
            _capsuleCollider2D.isTrigger = true;
            doorConstraint.CloseConstraint.enabled = true;
            doorConstraint.OpenConstraint.enabled = false;
        }
        
        // !-- Core Functionality
        private void OpenDoor() => StartCoroutine(OpenDoorRoutine());
        private void ActivateDoorTrigger() => StartCoroutine(ActivateDoorTriggerRoutine());
        
        private IEnumerator OpenDoorRoutine()
        {
            yield return new WaitForSeconds(cameraMoveInDelay);
            doorCameraAnimator.SetBool(MOVE_CAMERA_TRIGGRER, true);
            _playerController.SetDirection(gameObject.transform);
            CameraEventHandler.CameraShiftInEvent();
            
            yield return new WaitForSeconds(doorOpenDelay);
            _doorAnimator.SetTrigger(OPEN_DOOR_TRIGGER);
        }
        
        private IEnumerator ActivateDoorTriggerRoutine()
        {
            yield return new WaitForSeconds(cameraMoveOutDelay);
            doorCameraAnimator.SetBool(MOVE_CAMERA_TRIGGRER, false);
            CameraEventHandler.CameraShiftOutEvent();

            doorConstraint.OpenConstraint.enabled = true;
            doorConstraint.CloseConstraint.enabled = false;
            doorVirtualCamera.Follow = null;
        }
        
        private IEnumerator EnterDoorRoutine()
        {
            yield return new WaitForSeconds(0.1f);

            if (StageManager.Instance.CheckCanContinueStage())
                GameEventHandler.ContinueStageEvent();
            else
                GameEventHandler.GameWinEvent();
        }

        #endregion
        
        #region Collider Callbacks
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log($"{other.name} masuk portal!");
                StartCoroutine(EnterDoorRoutine());
            }
        }
        
        #endregion
    }
}