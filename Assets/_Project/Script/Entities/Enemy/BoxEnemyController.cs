using UnityEngine;

namespace LabirinKata.Entities.Enemy
{
    public class BoxEnemyController : EnemyBase
    {
        #region CariHuruf Callbacks
        
        protected override void RandomizeTargetPoint()
        {
            base.RandomizeTargetPoint();
            
            CurrentTargetIndex = EarlyPositionIndex >= MovePointTransform.Length - 1 ? 0 : EarlyPositionIndex + 1;
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
                switch (CurrentTargetIndex)
                {
                    case 0:
                        targetIndex = 1;
                        break;
                    case 1:
                        targetIndex = 2;
                        break;
                    case 2:
                        targetIndex = 3;
                        break;
                    case 3:
                        targetIndex = 0;
                        break;
                }

                CurrentTargetIndex = targetIndex;
                CurrentTarget = MovePointTransform[targetIndex];
            }
        }
        
        #endregion
    }
}