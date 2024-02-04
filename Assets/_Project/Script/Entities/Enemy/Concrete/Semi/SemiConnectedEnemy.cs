using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using KevinCastejon.MoreAttributes;

using Random = UnityEngine.Random;

namespace LabirinKata.Entities.Enemy
{
    public class SemiConnectedEnemy : EnemyBase
    {
        [Header("Semi-Linear")]
        [SerializeField] private int maxChange;
        [SerializeField] [ReadOnly] private int currentChange;
        [SerializeField] private float changeDirectionDelayTime;

        private int _maxTargetIndex;
        private bool _isDefaultWay;

         private bool CanChangeDirection
        {
            get => Random.value > 0.5f;
        }
        
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

                CurrentTarget = EnemeyPattern[CurrentPatternIndex].MovePointTransforms[CurrentTargetIndex];
            }
        }
        
        private IEnumerator ChangeDirection(float delayTime)
        {
            if (!CanChangeDirection) yield return null;

            var enemyMovePoint = EnemeyPattern[CurrentPatternIndex].MovePointTransforms;
            if (Vector2.Distance(transform.position , enemyMovePoint[_maxTargetIndex].position) < 0.01f)
            {
                var randomDirection = Random.Range(0, 1);

                switch (randomDirection)
                {
                    case 0:
                        CurrentTargetIndex = _maxTargetIndex - 1;
                        _isDefaultWay = false;
                        break;
                    case 1:
                        CurrentTargetIndex = 0;
                        _isDefaultWay = true;
                        break;
                }

                Debug.Log($"change direction: {_isDefaultWay}");
                yield return new WaitForSeconds(delayTime);
            }
        }
    }
}