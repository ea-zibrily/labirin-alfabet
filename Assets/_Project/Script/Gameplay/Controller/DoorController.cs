using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using Alphabet.Enum;
using Alphabet.Stage;
using Alphabet.Managers;
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
        [SerializeField] private GameObject doorVfx;
        
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
            // Objective
            GameEventHandler.OnObjectiveClear += OpenDoor;
            
            // Door
            _doorEventHandler.OnDoorOpen += ActivateDoorTrigger;
            _doorEventHandler.OnEffectPlaying += ActivateEffect;
        }
        
        private void OnDisable()
        {
            // Objective
            GameEventHandler.OnObjectiveClear -= OpenDoor;
            
            // Door
            _doorEventHandler.OnDoorOpen -= ActivateDoorTrigger;
            _doorEventHandler.OnEffectPlaying -= ActivateEffect;
        }

        private void Start()
        {
            InitializeDoor();
            doorVfx.SetActive(false);
        }
        
        #endregion
        
        #region Methods
        
        // !- Initialize
        private void InitializeDoor()
        {
            doorVirtualCamera.Follow = gameObject.transform;

            _capsuleCollider2D.isTrigger = true;
            doorConstraint.CloseConstraint.enabled = true;
            doorConstraint.OpenConstraint.enabled = false;
        }
        
        // !- Core
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

        private void EnterDoor()
        {
            if (StageManager.Instance.CheckCanContinueStage())
                GameEventHandler.ContinueStageEvent();
            else
                GameEventHandler.GameWinEvent();
        }
        
        private void ActivateEffect()
        {
            doorVfx.SetActive(true);
            if (doorVfx.TryGetComponent<ParticleSystem>(out var effect))
            {
                effect.Play();
                FindObjectOfType<AudioManager>().PlayAudio(Musics.LetterUISfx);
            }
        }

        #endregion
        
        #region Collider Callbacks
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                EnterDoor();
            }
        }
        
        #endregion
    }
}