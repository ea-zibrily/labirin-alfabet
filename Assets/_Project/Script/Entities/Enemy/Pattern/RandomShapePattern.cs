using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace LabirinKata.Entities.Enemy
{
    public class RandomShapePattern : PatternBase
    {
        private int _movePointLength;
        private int _maxTargetIndex;
        private bool _isDefaultWay;

        //-- Injected Fields
        private readonly float _delayTime;
        private readonly EnemyHelper _enemyHelper;

        public RandomShapePattern(Transform[] movePoint, EnemyBase enemyBase, EnemyHelper enemyHelper, float delay) : base(movePoint, enemyBase) 
        {
           _enemyHelper = enemyHelper;
           _delayTime = delay;
        }

        public override void InitializePattern(bool isReInitialize)
        {
            _isDefaultWay = true;
            _movePointLength = MovePointTransform.Length;
            _maxTargetIndex = _movePointLength - 1;

            EnemyBase.CurrentTargetIndex = EnemyBase.FirstPositionIndex < MovePointTransform.Length - 1 
                                ? EnemyBase.FirstPositionIndex + 1 : EnemyBase.FirstPositionIndex - 1;  
            EnemyBase.CurrentTarget = MovePointTransform[EnemyBase.CurrentTargetIndex];
        }

        public override void UpdatePattern()
        {
            if (Vector2.Distance(EnemyBase.transform.position, MovePointTransform[EnemyBase.CurrentTargetIndex].position) <= 0.01f)
            {
                EnemyBase.CurrentTargetIndex = _isDefaultWay ? 
                                    (EnemyBase.CurrentTargetIndex + 1) % _movePointLength :
                                    (EnemyBase.CurrentTargetIndex - 1 + _movePointLength) % _movePointLength;

                ChangeDirection(_delayTime);
                EnemyBase.CurrentTarget = MovePointTransform[EnemyBase.CurrentTargetIndex];
            }
        }

        private void ChangeDirection(float time) => EnemyBase.StartCoroutine(ChangeDirectionRoutine(time));

        private IEnumerator ChangeDirectionRoutine(float delayTime)
        {
            if (Vector2.Distance(EnemyBase.transform.position , MovePointTransform[_maxTargetIndex].position) <= 0.01f)
            {
                if (!_enemyHelper.IsChangeDirection()) yield break;

                EnemyBase.CurrentTargetIndex = _isDefaultWay ? _maxTargetIndex - 1 : 0;
                _isDefaultWay = !_isDefaultWay;
                EnemyBase.StopMovement();

                yield return new WaitForSeconds(delayTime);
                EnemyBase.StartMovement();
            }
        }
    }
}