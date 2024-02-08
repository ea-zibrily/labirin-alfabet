using System;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Enum;

using Random = UnityEngine.Random;

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

        [SerializeField] private EnemyPattern[] enemyPattern;
        [SerializeField] [ReadOnly] private int currentPatternIndex;

        private Transform[] _currentMovePattern;
        private bool _canChangePattern;

        [Header("Another Enemy Pattern")]
        private int _movePointLength;
        private int _maxTargetIndex;
        private bool _isDefaultWay; 

        #region Labirin Kata Callbacks

        // !-- Initialization
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();
            currentPatternIndex = Random.Range(0, enemyPattern.Length - 1);
            _canChangePattern = false;
            _currentMovePattern = enemyPattern[currentPatternIndex].MovePointTransforms;

            SetEnemyPosition(_currentMovePattern);
            SwitchInitializeType(enemyPattern[currentPatternIndex].PatternType);
        }

        // !-- Core Functionality
        protected override void EnemyPatternDirection()
        {
            base.EnemyPatternDirection();
            SwitchDirectionType(enemyPattern[currentPatternIndex].PatternType);
            
            if (!_canChangePattern) return;
            ChangeEnemyPattern();
        }

        private void ChangeEnemyPattern()
        {
            Debug.Log("bisa change ayo coba");
            var decisionPointIndex = enemyPattern[currentPatternIndex].DecisionPointIndex;

            if (Vector2.Distance(transform.position, _currentMovePattern[decisionPointIndex].position) <= 0.01f)
            {
                if (!EnemyHelper.IsChangeDirection()) return;

                currentPatternIndex += currentPatternIndex >= enemyPattern.Length - 1 ? -1 : 1;
                _currentMovePattern = enemyPattern[currentPatternIndex].MovePointTransforms;
                _canChangePattern = false;
                SwitchInitializeType(enemyPattern[currentPatternIndex].PatternType);

                Debug.LogWarning($"change pattern ke {currentPatternIndex} bertipe {enemyPattern[currentPatternIndex].PatternType}");
            }
        }
        
        // !-- Helper/Utilities
        private void SwitchInitializeType(EnemyPatternType type)
        {
             switch (type)
            {
                case EnemyPatternType.Line:
                    InitializeLineEnemy();
                    break;
                case EnemyPatternType.Connected:
                    InitializeConnectedEnemy();
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
                case EnemyPatternType.Connected:
                    ConnectedEnemyPatternDirection();
                    break;
            }
        }

        #endregion

        #region Line Pattern Callbacks
        
        private void InitializeLineEnemy()
        {   
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
                    _canChangePattern = isCurrentMax;
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

        #region Shape Pattern Callbacks

        private void InitializeConnectedEnemy()
        {   
            _movePointLength = _currentMovePattern.Length;
            
            CurrentTargetIndex = EarlyPositionIndex < _currentMovePattern.Length - 1 
                                ? EarlyPositionIndex + 1 : EarlyPositionIndex - 1;
            CurrentTarget = _currentMovePattern[CurrentTargetIndex];
        }

        private void ConnectedEnemyPatternDirection()
        {
            if (Vector2.Distance(transform.position, _currentMovePattern[CurrentTargetIndex].position) <= 0.01f)
            {
                _canChangePattern = CurrentTargetIndex >= _movePointLength - 1;
                CurrentTargetIndex = (CurrentTargetIndex + 1) % _movePointLength;
                CurrentTarget = _currentMovePattern[CurrentTargetIndex];
            }

        }

        #endregion
    }
}