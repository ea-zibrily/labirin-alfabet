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
        #region Fields & Properties
        
        [Header("Semi-Linear")]
        [SerializeField] private int maxChange;
        [SerializeField] private float changeDirectionDelayTime;
        
        private int _currentChange;
        private int _maxTargetIndex;
        private bool _isDefaultWay;
        private bool  _canChange;

        #endregion

        // !-- Initialization
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();

            if (IsPointLessThanThree()) return;
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

            _currentChange = 0;
            _canChange = true;
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
                StartCoroutine(ChangeDirectionRoutine(changeDirectionDelayTime));

                CurrentTarget = EnemeyPattern[CurrentPatternIndex].MovePointTransforms[CurrentTargetIndex];
            }
        }
        
        private IEnumerator ChangeDirectionRoutine(float delayTime)
        {
            if (_currentChange >= maxChange || !_canChange) yield break;

            var enemyMovePoint = EnemeyPattern[CurrentPatternIndex].MovePointTransforms;
            var randomPointIndex = Random.Range(1, Mathf.Max(2, enemyMovePoint.Count - 2));

            if (Vector2.Distance(transform.position , enemyMovePoint[randomPointIndex].position) < 0.01f)
            {
                if (!EnemyHelper.IsChangeDirection()) yield break;

                CurrentTargetIndex += _isDefaultWay ? -2 : 2;
                _isDefaultWay = !_isDefaultWay;
                IncreaseChangeCount();
                CanMove = false;

                yield return new WaitForSeconds(delayTime);
                CanMove = true;
            }
        }
        
        // !--Helpers/Utilities
        private void IncreaseChangeCount()
        {
            _currentChange++;
            if (_currentChange >= maxChange)
            {
                _currentChange = maxChange;
                _canChange = false;
            }
        }
        
        private void DecreaseChangeCount()
        {
            if (_currentChange < maxChange || _canChange) return;
            
            _currentChange--;
            if (_currentChange <= 0)
            {
                _currentChange = 0;
                _canChange = true;
            }
        }

        private bool IsPointLessThanThree()
        {
            return EnemeyPattern[CurrentPatternIndex].MovePointTransforms.Count - 1 < 2;
        }
    }
}