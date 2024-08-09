using System;
using System.Collections;
using UnityEngine;
using Alphabet.Enum;

using Random = UnityEngine.Random;

namespace Alphabet.Entities.Enemy
{
    public class Wanderer : EnemyBase
    {
        #region Struct
        [Serializable]
        public struct EnemyPattern
        {
            public EnemyPatternType PatternType;
            public Transform[] MovePointTransforms;
            public int DecisionPointIndex;
        }
        #endregion

        #region Fields & Properties

        [Header("Wanderer")]
        [SerializeField] private EnemyPattern[] enemyPattern;
        [SerializeField] private float changeDirectionDelayTime;

        private int _currentPatternIndex;
        private PatternBase[] _enemyPatterns;

        #endregion

        #region Labirin Kata Callbacks

        // !- Initialize
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();
            InitializePatternClass();

            _currentPatternIndex = Random.Range(0, enemyPattern.Length - 1);
            SetFirstPosition(enemyPattern[_currentPatternIndex].MovePointTransforms);

            SwitchPattern(_enemyPatterns[_currentPatternIndex]);
            InitializePattern();
        }

        private void InitializePatternClass()
        {
            _enemyPatterns = new PatternBase[enemyPattern.Length];

            for (var i = 0; i < _enemyPatterns.Length; i++)
            {
                var enemyType = enemyPattern[i].PatternType;
                var enemyMovePoint = enemyPattern[i].MovePointTransforms;
                var enemyDecisionPoint = enemyPattern[i].DecisionPointIndex;

                switch (enemyType)
                {
                    case EnemyPatternType.Line:
                        _enemyPatterns[i] = new LinePattern(enemyMovePoint, this, enemyDecisionPoint, isWanderer: true);
                        break;
                    case EnemyPatternType.Shape:
                        _enemyPatterns[i] = new ShapePattern(enemyMovePoint, this, enemyDecisionPoint, isWanderer: true);
                        break;
                }
            }
        }
        
        // !- Core
        protected override void EnemyPatternDirection()
        {
            base.EnemyPatternDirection();
            UpdatePattern();

            if (!CurrentPattern.CanChangePattern()) return;
            ChangeEnemyPattern();
        }

        private void ChangeEnemyPattern() => StartCoroutine(ChangeEnemyPatternRoutine());

        private IEnumerator ChangeEnemyPatternRoutine()
        {
            Debug.Log("bisa change ayo coba");
            var decisionPointIndex = enemyPattern[_currentPatternIndex].DecisionPointIndex;
            var currentMovePattern = enemyPattern[_currentPatternIndex].MovePointTransforms;

            if (Vector2.Distance(transform.position, currentMovePattern[decisionPointIndex].position) <= 0.01f)
            {
                if (!EnemyHelper.IsChangeDirection()) yield break;

                _currentPatternIndex += _currentPatternIndex >= enemyPattern.Length - 1 ? -1 : 1;
                
                SwitchPattern(_enemyPatterns[_currentPatternIndex]);
                ReInitializePattern();

                StopMovement();
                yield return new WaitForSeconds(changeDirectionDelayTime);
                StartMovement();
            }
        }

        #endregion
    }
}
