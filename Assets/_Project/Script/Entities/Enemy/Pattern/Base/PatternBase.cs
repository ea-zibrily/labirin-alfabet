using UnityEngine;

namespace Alphabet.Entities.Enemy
{
    public abstract class PatternBase
    {
        #region Fields & Property

        protected Transform[] MovePointTransform { get; private set;}
        protected EnemyBase EnemyBase { get; set;}
        protected int IterationCount { get; set;}

        private const int MAX_ITERATION = 2;

        #endregion

        #region Methods
        
        // !-- Initialize
        public PatternBase(Transform[] movePoint, EnemyBase enemyBase)
        {
            MovePointTransform = movePoint;
            EnemyBase = enemyBase;
        }

        public abstract void InitializePattern(bool isReInitialize);
        public abstract void UpdatePattern();

        // !-- Helper
        public bool CanChangePattern()
        {
            return IterationCount >= MAX_ITERATION;
        }

        #endregion
    }
}