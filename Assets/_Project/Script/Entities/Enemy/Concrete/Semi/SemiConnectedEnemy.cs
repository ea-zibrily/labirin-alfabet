using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

using Random = UnityEngine.Random;

namespace LabirinKata.Entities.Enemy
{
    public class SemiConnectedEnemy : EnemyBase
    {
        #region Fields & Properties

        [Header("Semi-Linear")]
        [SerializeField] private float changeDirectionDelayTime;

        private int _maxTargetIndex;
        private bool _isDefaultWay;

        #endregion
        
        // !-- Initialization
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();
            CurrentTargetIndex = EarlyPositionIndex >= EnemeyPattern[CurrentPatternIndex].MovePointTransforms.Count - 1 
                                ? EarlyPositionIndex - 1 : EarlyPositionIndex + 1;  

            _isDefaultWay = true;
            _maxTargetIndex = EnemeyPattern[CurrentPatternIndex].MovePointTransforms.Count - 1;
            CurrentTarget = EnemeyPattern[CurrentPatternIndex].MovePointTransforms[CurrentTargetIndex];
        }
        
        // !-- Core Functionality
        protected override void EnemyMove()
        {
            base.EnemyMove();
            // TODO: Test play jangan lupa lekku
            MoveEnemyPattern();
        }
        
        private void MoveEnemyPattern()
        {
            var maxTargetIndex = EnemeyPattern[CurrentPatternIndex].MovePointTransforms.Count;

            if (Vector2.Distance(transform.position, EnemeyPattern[CurrentPatternIndex].MovePointTransforms[CurrentTargetIndex].position) <= 0.01f)
            {
                CurrentTargetIndex = _isDefaultWay ? 
                                    (CurrentTargetIndex + 1) % maxTargetIndex :
                                    (CurrentTargetIndex - 1 + maxTargetIndex) % maxTargetIndex;

                StartCoroutine(ChangeDirectionRoutine(changeDirectionDelayTime));
                CurrentTarget = EnemeyPattern[CurrentPatternIndex].MovePointTransforms[CurrentTargetIndex];
            }
        }
        
        private IEnumerator ChangeDirectionRoutine(float delayTime)
        {
            var enemyMovePoint = EnemeyPattern[CurrentPatternIndex].MovePointTransforms;

            if (Vector2.Distance(transform.position , enemyMovePoint[_maxTargetIndex].position) < 0.01f)
            {
                if (!EnemyHelper.IsChangeDirection()) yield break;

                CurrentTargetIndex = _isDefaultWay ? _maxTargetIndex - 1 : 0;
                _isDefaultWay = !_isDefaultWay;
                CanMove = false;

                yield return new WaitForSeconds(delayTime);
                CanMove = true;
            }
        }
    }
}