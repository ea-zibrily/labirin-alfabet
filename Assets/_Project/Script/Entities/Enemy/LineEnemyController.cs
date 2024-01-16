using UnityEngine;

namespace LabirinKata.Entities.Enemy
{
    public class LineEnemyController : EnemyBase
    {
        #region CariHuruf Callbacks
        
        protected override void RandomizeTargetPoint()
        {
            base.RandomizeTargetPoint();
            
            CurrentTargetIndex = EarlyPositionIndex >= MovePointTransform.Length - 1 ? EarlyPositionIndex - 1 : EarlyPositionIndex + 1;
            CurrentTarget = MovePointTransform[CurrentTargetIndex];
        }
        
        protected override void EnemyMove()
        {
            base.EnemyMove();
            
            UpdateMovePattern();
        }
        
        private void UpdateMovePattern()
        {
            var targetIndex = 0;
            
            if (Vector2.Distance(transform.position, MovePointTransform[CurrentTargetIndex].position) <= 0.01f)
            {
                targetIndex = CurrentTargetIndex switch
                {
                    0 => 1,
                    1 => 0,
                    _ => targetIndex
                };

                CurrentTargetIndex = targetIndex;
                CurrentTarget = MovePointTransform[targetIndex];
            }
        }
        
        #endregion
    }
}