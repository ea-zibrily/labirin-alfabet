using System.Collections;
using LabirinKata.Gameplay.EventHandler;
using UnityEngine;

namespace LabirinKata.Gameplay.Controller
{
    public class DoorController : MonoBehaviour
    {
        #region Variable

        // Const Variable
        private const string OPEN_DOOR_TRIGGER = "Open";
        
        [Header("Door")] 
        [SerializeField] private float openDelayTime;
        
        [Header("Reference")]
        private BoxCollider2D _boxCollider2D;
        private Animator _doorAnimator;
        private DoorEventHandler _doorEventHandler;
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _doorAnimator = GetComponentInChildren<Animator>();
            _doorEventHandler = GetComponentInChildren<DoorEventHandler>();
        }
        
        private void OnEnable()
        {
            GameEventHandler.OnObjectiveClear += OpenDoor;
            _doorEventHandler.OnDoorOpen += ActivateTrigger;
        }
        
        private void OnDisable()
        {
            GameEventHandler.OnObjectiveClear -= OpenDoor;
            _doorEventHandler.OnDoorOpen -= ActivateTrigger;
        }

        private void Start()
        {
            InitializeDoor();
        }
        
        #endregion
        
        #region CariHuruf Callbacks
        
        //-- Initialization
        private void InitializeDoor() => _boxCollider2D.isTrigger = false;
        
        //-- Core Functionality
        private void OpenDoor() => StartCoroutine(OpenDoorRoutine());
        
        private IEnumerator OpenDoorRoutine()
        {
            yield return new WaitForSeconds(openDelayTime);
            _doorAnimator.SetTrigger(OPEN_DOOR_TRIGGER);
        }
        
        private void ActivateTrigger() => _boxCollider2D.isTrigger = true;
        
        #endregion
        
        #region Collider Callbacks
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) return;
            
            // if (GameManager.Instance.CanNextStage())
            // {
            //     GameEventHandler.GameWinEvent();
            // }
            // else
            // {
            //     GameEventHandler.ContinueStageEvent();
            // }
        }
        
        #endregion
    }
}