using System.Collections;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Alphabet.Entities.Enemy
{
    public class RandomLinePattern : PatternBase
    {
        #region Fields & Property
        private int _currentChange;
        private int _maxTargetIndex;
        private bool _isDefaultWay;
        private bool _canChangeDir;

        // Injected Fields
        private readonly int _maxChange;
        private readonly float _delayTime;
        private readonly EnemyHelper _enemyHelper;

        #endregion

        #region Methods

        // !- Initialize
        public RandomLinePattern(Transform[] movePoint, EnemyBase enemyBase, EnemyHelper enemyHelper, int maxChange, float delay) : base(movePoint, enemyBase) 
        {
            _maxChange = maxChange;
            _delayTime = delay;
            _enemyHelper  = enemyHelper;
        }

        public override void InitializePattern(bool isReInitialize)
        {
            _currentChange = 0;
            _canChangeDir = true;
            _maxTargetIndex = MovePointTransform.Length - 1;
            _isDefaultWay = EnemyBase.FirstPositionIndex < MovePointTransform.Length - 1;
            
            EnemyBase.CurrentTargetIndex = _isDefaultWay ? EnemyBase.FirstPositionIndex + 1 : EnemyBase.FirstPositionIndex - 1;
            EnemyBase.CurrentTarget = MovePointTransform[EnemyBase.CurrentTargetIndex];
        }

        // !- Core
        public override void UpdatePattern()
        {
            if (Vector2.Distance(EnemyBase.transform.position, MovePointTransform[EnemyBase.CurrentTargetIndex].position) <= 0.01f)
            {
                if (_isDefaultWay)
                {
                    var isCurrentMax = EnemyBase.CurrentTargetIndex >= _maxTargetIndex;

                    EnemyBase.CurrentTargetIndex += isCurrentMax ? -1 : 1;
                    _isDefaultWay = !isCurrentMax;
                }
                else
                {
                    var isCurrentZero = EnemyBase.CurrentTargetIndex <= 0;

                    EnemyBase.CurrentTargetIndex += isCurrentZero ? 1 : -1;
                    _isDefaultWay = isCurrentZero;
                }

                DecreaseChangeLength();
                ChangeDirection(_delayTime);
                
                EnemyBase.CurrentTarget = MovePointTransform[EnemyBase.CurrentTargetIndex];
            }
        }

        private void ChangeDirection(float time) => EnemyBase.StartCoroutine(ChangeDirectionRoutine(time));

        private IEnumerator ChangeDirectionRoutine(float delayTime)
        {
            if (_currentChange >= _maxChange || !_canChangeDir) yield break;

            var randomPointIndex = Random.Range(1, Mathf.Max(2, MovePointTransform.Length - 2));

            if (Vector2.Distance(EnemyBase.transform.position , MovePointTransform[randomPointIndex].position) < 0.01f)
            {
                if (!_enemyHelper.IsChangeDirection()) yield break;

                EnemyBase.CurrentTargetIndex += _isDefaultWay ? -2 : 2;
                _isDefaultWay = !_isDefaultWay;

                IncreaseChangeLength();
                EnemyBase.StopMovement();
                
                yield return new WaitForSeconds(delayTime);
                EnemyBase.StartMovement();
            }
        }
        
        // !- Helper
        private void IncreaseChangeLength()
        {
            _currentChange++;
            if (_currentChange >= _maxChange)
            {
                _currentChange = _maxChange;
                _canChangeDir = false;
            }
        }
        
        private void DecreaseChangeLength()
        {
            if (_currentChange < _maxChange || _canChangeDir) return;
            
            _currentChange--;
            if (_currentChange <= 0)
            {
                _currentChange = 0;
                _canChangeDir = true;
            }
        }

        #endregion
    }
}