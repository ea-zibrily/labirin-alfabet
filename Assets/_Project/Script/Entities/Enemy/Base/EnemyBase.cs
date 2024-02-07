using System;
using System.Collections.Generic;
using UnityEngine;
using LabirinKata.Data;
using LabirinKata.Enum;
using LabirinKata.Managers;

using Random = UnityEngine.Random;
using LabirinKata.Gameplay.Controller;

namespace LabirinKata.Entities.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        #region Struct

        [Serializable]
        public struct EnemyPattern
        {
            public string PatternNumber;
            public List<Transform> MovePointTransforms;
        }

        #endregion

        #region Const Variable

        private const string HORIZONTAL_KEY = "Horizontal";
        private const string VERTICAL_KEY = "Vertical";
        private const string IS_MOVE = "IsMove";

        #endregion

        #region Fields & Properties

        [Header("Data")] 
        public EnemyData EnemyData;
        public EnemyType EnemyType;
        [SerializeField] private EnemyPattern[] enemyPattern;

        public EnemyPattern[] EnemeyPattern => enemyPattern;
        protected int MaxEnemyPattern
        {
            get
            {
                var maxPattern = EnemyType switch
                {
                    EnemyType.Linear => 1,
                    EnemyType.SemiLinear => 1,
                    EnemyType.Multiple => 2,
                    _ => throw new ArgumentOutOfRangeException()
                };

                return maxPattern;
            }
        }

        protected bool CanMove;

        [Header("Target Point")]
        protected Transform CurrentTarget;
        protected int CurrentTargetIndex;

        protected int CurrentPatternIndex { get; set;}
        protected int EarlyPositionIndex { get; private set; }

        private Vector2 _enemyDirection;
        
        [Header("Reference")] 
        private Animator _enemyAnimator;
        protected EnemyHelper EnemyHelper { get; private set; }
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            EnemyHelper = new EnemyHelper();
            _enemyAnimator = GetComponentInChildren<Animator>();
        }
        
        private void OnEnable()
        {
            DoorController.OnCameraShiftIn += StopMovement;
            DoorController.OnCameraShiftOut += StartMovement;
        }

        private void OnDisable()
        {
            DoorController.OnCameraShiftIn -= StopMovement;
            DoorController.OnCameraShiftOut -= StartMovement;
        }

        private void Start()
        {
            InitializeEnemy();
        }
        
        private void Update()
        {
            if (CheckPatternCount() || !GameManager.Instance.IsGameStart) return;
            
            EnemyMove();
            EnemyAnimation();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        protected virtual void InitializeEnemy()
        {
            gameObject.transform.parent.name = EnemyData.EnemyName;
            StartMovement();

            //*-- Enemy Early Position
            CurrentPatternIndex = EnemeyPattern.Length > 1 ? 0 : Random.Range(0, EnemeyPattern.Length - 1);
            var enemyPoints = EnemeyPattern[CurrentPatternIndex].MovePointTransforms;

            EarlyPositionIndex = Random.Range(0, enemyPoints.Count - 1);
            transform.position = enemyPoints[EarlyPositionIndex].position;
        }
        
        // !-- Core Functionality
        protected virtual void EnemyMove()
        {
            var enemyPosition = transform.position;
            var targetPosition = CurrentTarget.position;
            var currentSpeed = EnemyData.EnemyMoveSpeed * Time.deltaTime;
            
            _enemyDirection = targetPosition - enemyPosition;
            _enemyDirection.Normalize();

            if (!CanMove) return;
            transform.position = Vector2.MoveTowards(enemyPosition, targetPosition, currentSpeed);
        }

        // TODO: Aktifken method animasi pas animasi dah ada lur
        private void EnemyAnimation()
        {
            if (CanMove)
            {
                _enemyAnimator.SetFloat(HORIZONTAL_KEY, _enemyDirection.x);
                _enemyAnimator.SetFloat(VERTICAL_KEY, _enemyDirection.y);
                _enemyAnimator.SetBool(IS_MOVE, true);
            }
            else
            {
                _enemyAnimator.SetBool(IS_MOVE, false);
            }
            
        }
        
        // !-- Helpers/Utilities
        public void StartMovement() => CanMove = true;
        public void StopMovement() => CanMove = false;

        private bool CheckPatternCount()
        {
            if (EnemeyPattern.Length > MaxEnemyPattern)
            {
                Debug.LogError("jumlah pattern kebanyakan kang \n 1. Linear & Semi-Linear max 1 \n 2. Multiple max 2");
                return true;
            }

            return false;
        }
        
        #endregion
        
    }
}