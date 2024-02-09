using System;
using System.Collections.Generic;
using UnityEngine;

namespace LabirinKata.Entities.Enemy
{
    public class LineTrapEnemy : EnemyBase
    {
        #region Fields & Properties
        
        [SerializeField] private Transform[] movePointTransforms;

        private int _maxTargetIndex;
        private bool _isDefaultWay; 

        #endregion

        #region Labirin Kata Callbacks
        
        // !-- Initialization
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();
            SetEnemyPosition(movePointTransforms);

            _maxTargetIndex = movePointTransforms.Length - 1;
            _isDefaultWay = EarlyPositionIndex < movePointTransforms.Length - 1;
            
            CurrentTargetIndex = _isDefaultWay ? EarlyPositionIndex + 1 : EarlyPositionIndex - 1;
            CurrentTarget = movePointTransforms[CurrentTargetIndex];
        }
        
        // !-- Core Functionality
        protected override void EnemyPatternDirection()
        {
            base.EnemyPatternDirection();
            if (Vector2.Distance(transform.position, movePointTransforms[CurrentTargetIndex].position) <= 0.01f)
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
                
                CurrentTarget = movePointTransforms[CurrentTargetIndex];
            }
        }

        #endregion
    }
}