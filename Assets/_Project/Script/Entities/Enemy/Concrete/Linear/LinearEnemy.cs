using System;
using System.Collections.Generic;
using UnityEngine;

namespace LabirinKata.Entities.Enemy
{
    public class LinearEnemy : EnemyBase
    {
        private int _maxTargetIndex;
        private bool _isDefaultWay;

        // !-- Initialization
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();
            if (EarlyPositionIndex >= EnemeyPattern[CurrentPatternIndex].MovePointTransforms.Count - 1)
            {
                CurrentTargetIndex = EarlyPositionIndex - 1;
                _isDefaultWay = false;
            }
            else
            {
                CurrentTargetIndex = EarlyPositionIndex + 1;
                _isDefaultWay = true;
            }

            _maxTargetIndex = EnemeyPattern[CurrentPatternIndex].MovePointTransforms.Count - 1;
            CurrentTarget = EnemeyPattern[CurrentPatternIndex].MovePointTransforms[CurrentTargetIndex];
        }
        
        // !-- Core Functionality
        protected override void EnemyMove()
        {
            base.EnemyMove();
            MoveEnemyPattern();
        }
        
        private void MoveEnemyPattern()
        {
            if (Vector2.Distance(transform.position, EnemeyPattern[CurrentPatternIndex].MovePointTransforms[CurrentTargetIndex].position) <= 0.01f)
            {
                if (_isDefaultWay)
                {
                    var isCurrentMax = CurrentTargetIndex >= _maxTargetIndex;

                    CurrentTargetIndex += isCurrentMax ? -1 : 1;
                    _isDefaultWay = !isCurrentMax;
                }
                else
                {
                    var isCurrentZero = CurrentTargetIndex <= 0;

                    CurrentTargetIndex += isCurrentZero ? 1 : -1;
                    _isDefaultWay = isCurrentZero;
                }

                CurrentTarget = EnemeyPattern[CurrentPatternIndex].MovePointTransforms[CurrentTargetIndex];
            }
        }
    }
}