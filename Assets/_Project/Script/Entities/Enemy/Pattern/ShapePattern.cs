using System;
using System.Collections.Generic;
using UnityEngine;

namespace Alphabet.Entities.Enemy
{
    public class ShapePattern : PatternBase
    {
        #region Fields & Property

        private int _movePointLength;

        // --Injected Fields
        private readonly bool _isWanderer;
        private readonly int _decisionPointIndex;

        #endregion

        public ShapePattern(Transform[] movePoint, EnemyBase enemyBase, int decision, bool isWanderer) : base(movePoint, enemyBase) 
        {
            _decisionPointIndex = decision;
            _isWanderer = isWanderer;
        }

        public override void InitializePattern(bool isReInitialize)
        {
            IterationCount = 0;
            if (isReInitialize) EnemyBase.FirstPositionIndex = _decisionPointIndex;
            
            _movePointLength = MovePointTransform.Length;
            
            EnemyBase.CurrentTargetIndex = EnemyBase.FirstPositionIndex < MovePointTransform.Length - 1 
                                ? EnemyBase.FirstPositionIndex + 1 : EnemyBase.FirstPositionIndex - 1;
            EnemyBase.CurrentTarget = MovePointTransform[EnemyBase.CurrentTargetIndex];
        }

        public override void UpdatePattern()
        {
            if (Vector2.Distance(EnemyBase.transform.position, MovePointTransform[EnemyBase.CurrentTargetIndex].position) <= 0.01f)
            {
                var maxTargetIndex = _movePointLength - 1;

                EnemyBase.CurrentTargetIndex = (EnemyBase.CurrentTargetIndex + 1) % _movePointLength;
                EnemyBase.CurrentTarget = MovePointTransform[EnemyBase.CurrentTargetIndex];
                if (_isWanderer) IterationCount += EnemyBase.CurrentTargetIndex >=  maxTargetIndex ? 1 : 0;
            }
        }
    }
}