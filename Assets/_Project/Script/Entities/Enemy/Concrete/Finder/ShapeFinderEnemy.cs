using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

using Random = UnityEngine.Random;

namespace LabirinKata.Entities.Enemy
{
    public class ShapeFinderEnemy : EnemyBase
    {
        #region Fields & Properties

        [SerializeField] private Transform[] movePointTransforms;
        [SerializeField] private float changeDirectionDelayTime;

        private int _movePointLength;
        private int _maxTargetIndex;
        private bool _isDefaultWay;
        
        #endregion
        
        // !-- Initialization
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();
            SetEnemyPosition(movePointTransforms);

            _isDefaultWay = true;
            _movePointLength = movePointTransforms.Length;
            _maxTargetIndex = _movePointLength - 1;

            CurrentTargetIndex = EarlyPositionIndex < movePointTransforms.Length - 1 
                                ? EarlyPositionIndex + 1 : EarlyPositionIndex - 1;  
            CurrentTarget = movePointTransforms[CurrentTargetIndex];
        }

        // !-- Core Functionality
        protected override void EnemyPatternDirection()
        {
            base.EnemyPatternDirection();
            if (Vector2.Distance(transform.position, movePointTransforms[CurrentTargetIndex].position) <= 0.01f)
            {
                CurrentTargetIndex = _isDefaultWay ? 
                                    (CurrentTargetIndex + 1) % _movePointLength :
                                    (CurrentTargetIndex - 1 + _movePointLength) % _movePointLength;

                StartCoroutine(ChangeDirectionRoutine(changeDirectionDelayTime));
                CurrentTarget = movePointTransforms[CurrentTargetIndex];
            }
        }
        
        private IEnumerator ChangeDirectionRoutine(float delayTime)
        {
            if (Vector2.Distance(transform.position , movePointTransforms[_maxTargetIndex].position) <= 0.01f)
            {
                if (!EnemyHelper.IsChangeDirection()) yield break;

                CurrentTargetIndex = _isDefaultWay ? _maxTargetIndex - 1 : 0;
                _isDefaultWay = !_isDefaultWay;
                StopMovement();

                yield return new WaitForSeconds(delayTime);
                StartMovement();
            }
        }
    }
}