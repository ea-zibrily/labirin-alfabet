using System;
using System.Collections.Generic;
using UnityEngine;

namespace Alphabet.Entities.Enemy
{
    public abstract class PatternBase
    {
        #region Fields & Property

        protected Transform[] MovePointTransform { get; private set;}
        protected EnemyBase EnemyBase { get; set;}
        protected int IterationCount { get; set;}

        //-- Constant Variable
        private const int MAX_ITERATION = 2;

        #endregion

        #region Labirin Kata Callbacks
        
        // !-- Initialization
        public PatternBase(Transform[] movePoint, EnemyBase enemyBase)
        {
            MovePointTransform = movePoint;
            EnemyBase = enemyBase;
        }

        public abstract void InitializePattern(bool isReInitialize);
        public abstract void UpdatePattern();

        // !-- Helper/Utilities
        public bool CanChangePattern()
        {
            return IterationCount >= MAX_ITERATION;
        }

        #endregion
    }
}