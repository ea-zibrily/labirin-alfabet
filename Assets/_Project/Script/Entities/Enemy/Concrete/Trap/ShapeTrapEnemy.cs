using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabirinKata.Entities.Enemy
{
    public class ShapeTrapEnemy : EnemyBase
    {
        [SerializeField] private Transform[] movePointTransforms;
        
        private int _movePointLength;

        #region Labirin Kata CaLlbacks

        // !-- Initialization
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();
            SetEnemyPosition(movePointTransforms);
            
            _movePointLength = movePointTransforms.Length;

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
                CurrentTargetIndex = (CurrentTargetIndex + 1) % _movePointLength;
                CurrentTarget = movePointTransforms[CurrentTargetIndex];
            }

        }

        #endregion
    }
}
