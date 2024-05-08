using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using Alphabet.Stage;
using Alphabet.Entities.Player;
using Alphabet.Gameplay.EventHandler;
using Alphabet.Managers;

namespace Alphabet.Gameplay.Controller
{
    public class HoleController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Hole")] 
        [SerializeField] private float holeOpenDelay;
        [SerializeField] private float reachHoleDelay;
        
        [Header("Camera")]
        [SerializeField] private float cameraMoveInDelay;
        [SerializeField] private float cameraMoveOutDelay;
        [SerializeField] private CinemachineVirtualCamera holeVirtualCamera;
        [SerializeField] private Animator holeCameraAnimator;

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
        
        #region CariHuruf Callbacks
        
        // !-- Initialization
        private void InitializeDoor()
        {
            holeVirtualCamera.Follow = gameObject.transform;
            _capsuleCollider2D.enabled = false;
            _capsuleCollider2D.isTrigger = false;
        }
        
        // !-- Core Functionality
        private void OpenDoor() => StartCoroutine(OpenDoorRoutine());
        private void ActivateDoorTrigger() => StartCoroutine(ActivateDoorTriggerRoutine());
        
        private IEnumerator OpenDoorRoutine()
        {
            yield return new WaitForSeconds(cameraMoveInDelay);
            holeCameraAnimator.SetBool(MOVE_CAMERA_TRIGGRER, true);
            _playerController.SetDirection(gameObject.transform);
            CameraEventHandler.CameraShiftInEvent();
            
            yield return new WaitForSeconds(holeOpenDelay);
            _doorAnimator.SetTrigger(OPEN_DOOR_TRIGGER);
        }
        
        private IEnumerator ActivateDoorTriggerRoutine()
        {
            yield return new WaitForSeconds(cameraMoveOutDelay);
            holeCameraAnimator.SetBool(MOVE_CAMERA_TRIGGRER, false);
            CameraEventHandler.CameraShiftOutEvent();

            _capsuleCollider2D.enabled = true;
            _capsuleCollider2D.isTrigger = true;
            holeVirtualCamera.Follow = null;
        }
        
        private IEnumerator EnterDoorRoutine()
        {
            yield return new WaitForSeconds(0.5f);
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
        
        #region Collider Callbacks
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerController player))
            {
                Debug.Log($"{player.name} masuk portal!");
                StartCoroutine(EnterDoorRoutine());
            }
        }
        
        #endregion
    }
}