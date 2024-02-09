using System;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Enum;

using Random = UnityEngine.Random;
using System.Collections;

namespace LabirinKata.Entities.Enemy
{
    public class WandererEnemy : EnemyBase
    {
        #region Struct
        [Serializable]
        public struct EnemyPattern
        {
            public EnemyPatternType PatternType;
            public Transform[] MovePointTransforms;
            public int DecisionPointIndex;
        }
        #endregion

        #region Fields & Properties

        [SerializeField] private EnemyPattern[] enemyPattern;
        [SerializeField] private float changeDirectionDelayTime;
        [SerializeField] [ReadOnly] private int currentPatternIndex;

        private Transform[] _currentMovePattern;
        private int _iterationCount;

        [Header("Another Enemy Pattern")]
        private int _movePointLength;
        private int _maxTargetIndex;
        private bool _isDefaultWay; 

        #endregion

        #region Labirin Kata Callbacks

        // !-- Initialization
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();
            currentPatternIndex = Random.Range(0, enemyPattern.Length - 1);
            _iterationCount = 0;
            _currentMovePattern = enemyPattern[currentPatternIndex].MovePointTransforms;

            SetEnemyPosition(_currentMovePattern);
            SwitchInitializeType(enemyPattern[currentPatternIndex].PatternType);
        }

        // !-- Core Functionality
        protected override void EnemyPatternDirection()
        {
            base.EnemyPatternDirection();
            SwitchDirectionType(enemyPattern[currentPatternIndex].PatternType);
            
            if (!CanChangePattern()) return;
            ChangeEnemyPattern();
        }

        private void ChangeEnemyPattern() => StartCoroutine(ChangeEnemyPatternRoutine());

        private IEnumerator ChangeEnemyPatternRoutine()
        {
            Debug.Log("bisa change ayo coba");
            var decisionPointIndex = enemyPattern[currentPatternIndex].DecisionPointIndex;

            if (Vector2.Distance(transform.position, _currentMovePattern[decisionPointIndex].position) <= 0.01f)
            {
                if (!EnemyHelper.IsChangeDirection()) yield break;

                currentPatternIndex += currentPatternIndex >= enemyPattern.Length - 1 ? -1 : 1;
                _currentMovePattern = enemyPattern[currentPatternIndex].MovePointTransforms;
                _iterationCount = 0;

                SwitchReInitializeType(enemyPattern[currentPatternIndex].PatternType);
                StopMovement();
                Debug.LogWarning($"change pattern ke {currentPatternIndex} bertipe {enemyPattern[currentPatternIndex].PatternType}");

                yield return new WaitForSeconds(changeDirectionDelayTime);
                StartMovement();
                Debug.LogWarning("gas maju lagi");
            }
        }
        
        // !-- Helper/Utilities
        private bool CanChangePattern()
        {
            return _iterationCount >= 2;
        }

        private void SwitchInitializeType(EnemyPatternType type)
        {
             switch (type)
            {
                case EnemyPatternType.Line:
                    InitializeLineEnemy();
                    break;
                case EnemyPatternType.Shape:
                    InitializeConnectedEnemy();
                    break;
            }
        }

        private void SwitchReInitializeType(EnemyPatternType type)
        {
             switch (type)
            {
                case EnemyPatternType.Line:
                    ReInitializeLineEnemy();
                    break;
                case EnemyPatternType.Shape:
                    ReInitializeShapeEnemy();
                    break;
            }
        }

        private void SwitchDirectionType(EnemyPatternType type)
        {
            switch (type)
            {
                case EnemyPatternType.Line:
                    LineEnemyPatternDirection();
                    break;
                case EnemyPatternType.Shape:
                    ShapeEnemyPatternDirection();
                    break;
            }
        }

        #endregion

        #region Line Pattern
        
        private void InitializeLineEnemy()
        {   
            _maxTargetIndex = _currentMovePattern.Length - 1;
            _isDefaultWay = EarlyPositionIndex < _currentMovePattern.Length - 1;
            
            CurrentTargetIndex = _isDefaultWay ? EarlyPositionIndex + 1 : EarlyPositionIndex - 1;
            CurrentTarget = _currentMovePattern[CurrentTargetIndex];
        }

        private void ReInitializeLineEnemy()
        {
            EarlyPositionIndex = enemyPattern[currentPatternIndex].DecisionPointIndex;
            _maxTargetIndex = _currentMovePattern.Length - 1;
            _isDefaultWay = EarlyPositionIndex < _currentMovePattern.Length - 1;
            
            CurrentTargetIndex = _isDefaultWay ? EarlyPositionIndex + 1 : EarlyPositionIndex - 1;
            CurrentTarget = _currentMovePattern[CurrentTargetIndex];
        }

        private void LineEnemyPatternDirection()
        {
            if (Vector2.Distance(transform.position, _currentMovePattern[CurrentTargetIndex].position) <= 0.01f)
            {
                if (_isDefaultWay)
                {
                    var isCurrentMax = CurrentTargetIndex >= _maxTargetIndex;

                    CurrentTargetIndex += isCurrentMax ? -1 : 1;
                    _iterationCount += isCurrentMax ? 1 : 0;
                    _isDefaultWay = !isCurrentMax;
                }
                else
                {
                    var isCurrentZero = CurrentTargetIndex <= 0;

                    CurrentTargetIndex += isCurrentZero ? 1 : -1;
                    _isDefaultWay = isCurrentZero;
                }
                
                CurrentTarget = _currentMovePattern[CurrentTargetIndex];
            }
        }

        #endregion

        #region Shape Pattern

        private void InitializeConnectedEnemy()
        {   
            _movePointLength = _currentMovePattern.Length;
            
            CurrentTargetIndex = EarlyPositionIndex < _currentMovePattern.Length - 1 
                                ? EarlyPositionIndex + 1 : EarlyPositionIndex - 1;
            CurrentTarget = _currentMovePattern[CurrentTargetIndex];
        }

        private void ReInitializeShapeEnemy()
        {   
            EarlyPositionIndex = enemyPattern[currentPatternIndex].DecisionPointIndex;
            _movePointLength = _currentMovePattern.Length;
            _maxTargetIndex = _currentMovePattern.Length - 1;
            
            CurrentTargetIndex = EarlyPositionIndex < _currentMovePattern.Length - 1 
                                ? EarlyPositionIndex + 1 : EarlyPositionIndex - 1;
            CurrentTarget = _currentMovePattern[CurrentTargetIndex];
        }

        private void ShapeEnemyPatternDirection()
        {
            if (Vector2.Distance(transform.position, _currentMovePattern[CurrentTargetIndex].position) <= 0.01f)
            {
                CurrentTargetIndex = (CurrentTargetIndex + 1) % _movePointLength;
                CurrentTarget = _currentMovePattern[CurrentTargetIndex];

                _iterationCount += CurrentTargetIndex >=  _maxTargetIndex ? 1: 0;
            }

        }

        #endregion
    }
}