using UnityEngine;

namespace LabirinKata.Entities.Enemy
{
    public class ElbowEnemyController : EnemyBase
    {
        #region Variable

        private bool _isDefaultWay;
        
        #endregion
        
        #region CariHuruf Callbacks
        
        protected override void RandomizeTargetPoint()
        {
            base.RandomizeTargetPoint();
            
            if (EarlyPositionIndex >= MovePointTransform.Length - 1)
            {
                CurrentTargetIndex = EarlyPositionIndex - 1;
                _isDefaultWay = false;
            }
            else
            {
                CurrentTargetIndex = EarlyPositionIndex + 1;
                _isDefaultWay = true;
            }
            
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
                        _isDefaultWay = true;
                        break;
                    case 1:
                        targetIndex = _isDefaultWay ? 2 : 0;
                        break;
                    case 2:
                        targetIndex = 1;
                        _isDefaultWay = false;
                        break;
                }
                
                CurrentTargetIndex = targetIndex;
                CurrentTarget = MovePointTransform[targetIndex];
            }
        }
        
        #endregion
    }
}