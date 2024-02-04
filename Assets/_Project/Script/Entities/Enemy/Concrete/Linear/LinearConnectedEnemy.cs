using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabirinKata.Entities.Enemy
{
    public class LinearConnectedEnemy : EnemyBase
    {
        // !-- Initialization
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();
            
            CurrentTargetIndex = EarlyPositionIndex >= EnemeyPattern[CurrentPatternIndex].MovePointTransforms.Count - 1 
                                ? EarlyPositionIndex - 1 : EarlyPositionIndex + 1;

            CurrentTarget = EnemeyPattern[CurrentPatternIndex].MovePointTransforms[CurrentTargetIndex];
        }
        
        // !-- Core Functionality
        protected override void EnemyMove()
        {
            base.EnemyMove();
            MoveEnemyPattern();
        }
        
        private void MoveEnemyPattern()
        {
            var maxTargetIndex = EnemeyPattern[CurrentPatternIndex].MovePointTransforms.Count;
            if (Vector2.Distance(transform.position, EnemeyPattern[CurrentPatternIndex].MovePointTransforms[CurrentTargetIndex].position) <= 0.01f)
            {
                CurrentTargetIndex = (CurrentTargetIndex + 1) % maxTargetIndex;
                CurrentTarget = EnemeyPattern[CurrentPatternIndex].MovePointTransforms[CurrentTargetIndex];
            }
        }
    }
}
