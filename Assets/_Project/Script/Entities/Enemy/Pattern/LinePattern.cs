using System;
using System.Collections.Generic;
using UnityEngine;

namespace LabirinKata.Entities.Enemy
{
    public class LinePattern : PatternBase
    {
        #region Fields & Property

        private int _maxTargetIndex;
        private bool _isDefaultWay;
        
        //-- Injected Fields
        private readonly int _decisionPointIndex;
        private readonly bool _isWanderer;

        #endregion

        public LinePattern(Transform[] movePoint, EnemyBase enemyBase, int decision, bool isWanderer) : base(movePoint, enemyBase) 
        {
            _decisionPointIndex = decision;
            _isWanderer = isWanderer;
        }

        public override void InitializePattern(bool isReInitialize)
        {
            IterationCount = 0;
            if (isReInitialize) EnemyBase.FirstPositionIndex = _decisionPointIndex;

            _maxTargetIndex = MovePointTransform.Length - 1;
            _isDefaultWay = EnemyBase.FirstPositionIndex < _maxTargetIndex;

            EnemyBase.CurrentTargetIndex = _isDefaultWay ? EnemyBase.FirstPositionIndex + 1 : EnemyBase.FirstPositionIndex - 1;
            EnemyBase.CurrentTarget = MovePointTransform[EnemyBase.CurrentTargetIndex];
        }
        
        public override void UpdatePattern()
        {
            if (Vector2.Distance(EnemyBase.transform.position, MovePointTransform[EnemyBase.CurrentTargetIndex].position) <= 0.01f)
            {
                if (_isDefaultWay)
                {
                    var isCurrentMax = EnemyBase.CurrentTargetIndex >= _maxTargetIndex;

                    EnemyBase.CurrentTargetIndex += isCurrentMax ? -1 : 1;
                    _isDefaultWay = !isCurrentMax;
                    if (_isWanderer) IterationCount += isCurrentMax ? 1 : 0;
                }
                else
                {
                    var isCurrentZero = EnemyBase.CurrentTargetIndex <= 0;

                    EnemyBase.CurrentTargetIndex += isCurrentZero ? 1 : -1;
                    _isDefaultWay = isCurrentZero;
                }
                
                EnemyBase.CurrentTarget = MovePointTransform[EnemyBase.CurrentTargetIndex];
            }
        }
    }
}