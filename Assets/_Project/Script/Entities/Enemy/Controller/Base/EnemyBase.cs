﻿using System;
using System.Collections.Generic;
using UnityEngine;
using LabirinKata.Data;
using LabirinKata.Enum;
using LabirinKata.Managers;
using LabirinKata.Gameplay.EventHandler;

using Random = UnityEngine.Random;

namespace LabirinKata.Entities.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        #region Const Variable

        private const string HORIZONTAL_KEY = "Horizontal";
        private const string VERTICAL_KEY = "Vertical";
        private const string IS_MOVE = "IsMove";

        #endregion

        #region Fields & Properties

        [Header("Data")] 
        public EnemyData EnemyData;

        private bool _canMove;
        private Vector2 _enemyDirection;

        protected PatternBase CurrentPattern { get; set; }
        public Transform CurrentTarget { get; set; }
        public int CurrentTargetIndex { get; set; }
        public int FirstPositionIndex { get; set; }
        
        [Header("Reference")] 
        private Animator _enemyAnimator;
        protected EnemyHelper EnemyHelper { get; private set; }
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _enemyAnimator = GetComponentInChildren<Animator>();
            EnemyHelper = new EnemyHelper();
        }
        
        private void OnEnable()
        {
            CameraEventHandler.OnCameraShiftIn += StopMovement;
            CameraEventHandler.OnCameraShiftOut += StartMovement;
        }

        private void OnDisable()
        {
            CameraEventHandler.OnCameraShiftIn -= StopMovement;
            CameraEventHandler.OnCameraShiftOut -= StartMovement;
        }

        private void Start()
        {
            InitializeEnemy();
        }
        
        private void Update()
        {
            if (!GameManager.Instance.IsGameStart) return;

            EnemyMove();
            EnemyPatternDirection();
            EnemyAnimation();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        protected virtual void InitializeEnemy() 
        {
            transform.parent.name = EnemyData.EnemyName;
            StartMovement();
        }

        protected void SetFirstPosition(Transform[] wayPoints)
        {
            FirstPositionIndex = Random.Range(0, wayPoints.Length - 1);
            transform.position = wayPoints[FirstPositionIndex].position;
        }
        
        // !-- Core Functionality
        protected virtual void EnemyPatternDirection() { }

        private void EnemyMove()
        {
            var enemyPosition = transform.position;
            var targetPosition = CurrentTarget.position;
            var currentSpeed = EnemyData.EnemyMoveSpeed * Time.deltaTime;
            
            _enemyDirection = targetPosition - enemyPosition;
            _enemyDirection.Normalize();

            if (!_canMove) return;
            transform.position = Vector2.MoveTowards(enemyPosition, targetPosition, currentSpeed);
        }

        private void EnemyAnimation()
        {
            if (_canMove)
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
        public void StartMovement() => _canMove = true;
        public void StopMovement() => _canMove = false;

        protected void SwitchPattern(PatternBase newPattern)
        {
            CurrentPattern = newPattern;
        }

        protected void InitializePattern() 
        {
            CurrentPattern.InitializePattern(isReInitialize: false);
        }

        protected void ReInitializePattern() 
        {
            CurrentPattern.InitializePattern(isReInitialize: true);
        }
        
        protected void UpdatePattern()
        {
            CurrentPattern.UpdatePattern();
        }
        
        #endregion
        
    }
}