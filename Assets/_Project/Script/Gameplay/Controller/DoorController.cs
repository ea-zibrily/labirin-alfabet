using System.Collections;
using Cinemachine;
using LabirinKata.Entities.Player;
using LabirinKata.Gameplay.EventHandler;
using LabirinKata.Managers;
using LabirinKata.Stage;
using UnityEngine;

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
        [SerializeField] private float openDelayTime;
        
        [Header("Camera")]
        [SerializeField] private CinemachineVirtualCamera doorVirtualCamera;
        [SerializeField] private Animator doorCameraAnimator;
        
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
        
        //-- Initialization
        private void InitializeDoor()
        {
            doorVirtualCamera.transform.position = transform.position;
            _boxCollider2D.isTrigger = false;
        }

        //-- Core Functionality
        private void OpenDoor() => StartCoroutine(OpenDoorRoutine());
        private void ActivateDoorTrigger() => StartCoroutine(ActivateDoorTriggerRoutine());
        
        private IEnumerator OpenDoorRoutine()
        {
            doorCameraAnimator.SetBool(MOVE_CAMERA_TRIGGRER, true);
            _playerController.StopMovement();
            
            yield return new WaitForSeconds(openDelayTime);
            _doorAnimator.SetTrigger(OPEN_DOOR_TRIGGER);
        }
        
        private IEnumerator ActivateDoorTriggerRoutine()
        {
            yield return new WaitForSeconds(openDelayTime);
            doorCameraAnimator.SetBool(MOVE_CAMERA_TRIGGRER, false);
            _playerController.StartMovement();
            
            _boxCollider2D.isTrigger = true;
            doorVirtualCamera.transform.position = Vector3.zero;
        }
        
        #endregion
        
        #region Collider Callbacks
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) return;
            
            if (StageManager.Instance.CanContinueStage)
            {
                GameEventHandler.GameWinEvent();
            }
            else
            {
                GameEventHandler.ContinueStageEvent();
            }
        }
        
        #endregion
    }
}