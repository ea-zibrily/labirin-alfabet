using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace LabirinKata.Entities.Enemy
{
    public class LineFinderEnemy : EnemyBase
    {
        #region Fields & Properties
        
        [SerializeField] private Transform[] movePointTransforms;
        [SerializeField] private int maxChange;
        [SerializeField] private float changeDirectionDelayTime;
        
        private int _currentChange;
        private int _maxTargetIndex;
        private bool _isDefaultWay;
        private bool _canChangeDir;

        #endregion

        #region Labirin Kata Callbacks
        
        // !-- Initialization
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();
            if (IsPointLessThanThree()) return;

            SetEnemyPosition(movePointTransforms);

            _currentChange = 0;
            _canChangeDir = true;
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
                DecreaseChangeLength();
                StartCoroutine(ChangeDirectionRoutine(changeDirectionDelayTime));

                CurrentTarget = movePointTransforms[CurrentTargetIndex];
            }
        }
        
        private IEnumerator ChangeDirectionRoutine(float delayTime)
        {
            if (_currentChange >= maxChange || !_canChangeDir) yield break;

            var randomPointIndex = Random.Range(1, Mathf.Max(2, movePointTransforms.Length - 2));

            if (Vector2.Distance(transform.position , movePointTransforms[randomPointIndex].position) < 0.01f)
            {
                if (!EnemyHelper.IsChangeDirection()) yield break;

                CurrentTargetIndex += _isDefaultWay ? -2 : 2;
                _isDefaultWay = !_isDefaultWay;

                IncreaseChangeLength();
                StopMovement();

                yield return new WaitForSeconds(delayTime);
                StartMovement();
            }
        }
        
        // !--Helpers/Utilities
        private void IncreaseChangeLength()
        {
            _currentChange++;
            if (_currentChange >= maxChange)
            {
                _currentChange = maxChange;
                _canChangeDir = false;
            }
        }
        
        private void DecreaseChangeLength()
        {
            if (_currentChange < maxChange || _canChangeDir) return;
            
            _currentChange--;
            if (_currentChange <= 0)
            {
                _currentChange = 0;
                _canChangeDir = true;
            }
        }

        private bool IsPointLessThanThree()
        {
            return movePointTransforms.Length - 1 < 2;
        }

        #endregion
    }
}