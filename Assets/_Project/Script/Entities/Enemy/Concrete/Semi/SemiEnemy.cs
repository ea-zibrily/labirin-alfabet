using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.MoreAttributes;

using Random = UnityEngine.Random;

namespace LabirinKata.Entities.Enemy
{
    public class SemiEnemy : EnemyBase
    {
        #region  Fields & Properties
        
        [Header("Semi-Linear")]
        [SerializeField] private int maxChange;
        [SerializeField] [ReadOnly] private int currentChange;
        [SerializeField] private float changeDirectionDelayTime;
        
        private int _maxTargetIndex;
        private int _randomPointIndex;
        private bool _isDefaultWay;
        private bool  _hasCurrentChangeZero;

        private bool CanChangeDirection
        {
            get => Random.value > 0.5f;
        }

        #endregion

        // !-- Initialization
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();

            if (IsPointLessThanThree())
            {
                Debug.LogError("move pointnya kurenx dr 3 banh");
                return;
            }

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

            currentChange = 0;
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
                DecreaseChangeCount();
                StartCoroutine(ChangeDirection(changeDirectionDelayTime));

                CurrentTarget = EnemeyPattern[CurrentPatternIndex].MovePointTransforms[CurrentTargetIndex];
            }
        }
        
        private IEnumerator ChangeDirection(float delayTime)
        {
            if (!CanChangeDirection ||_hasCurrentChangeZero) yield return null;

            var enemyMovePoint = EnemeyPattern[CurrentPatternIndex].MovePointTransforms;
            _randomPointIndex = _maxTargetIndex < 2 ? 1 : Random.Range(1, enemyMovePoint.Count - 2);
            
            if (Vector2.Distance(transform.position , enemyMovePoint[_randomPointIndex].position) < 0.01f)
            {
                var randomDirection = Random.Range(0, 1);

                CurrentTargetIndex = randomDirection is 1 ? 1 : -1;
                _isDefaultWay = !_isDefaultWay;
                currentChange++;
                _hasCurrentChangeZero = false;
                Debug.Log("change direction");

                yield return new WaitForSeconds(delayTime);
            }
        }
        
        // !--Helpers/Utilities
        private void DecreaseChangeCount()
        {
            if (currentChange < maxChange || _hasCurrentChangeZero) return;
            
            currentChange--;
            if (currentChange <= 0)
            {
                currentChange = 0;
                _hasCurrentChangeZero = true;
            }
        }

        private bool IsPointLessThanThree()
        {
            return EnemeyPattern[CurrentPatternIndex].MovePointTransforms.Count - 1 > 1;
        }
    }
}