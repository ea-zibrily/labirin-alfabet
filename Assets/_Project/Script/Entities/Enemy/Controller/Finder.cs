using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alphabet.Enum;

namespace Alphabet.Entities.Enemy
{
    [AddComponentMenu("Alphabet/Entities/Enemy/Finder")]
    public class Finder : EnemyBase
    {
        #region Fields & Property
        
        [Header("Finder")]
        [SerializeField] private EnemyPatternType patternType;
        [SerializeField] private Transform[] movePointTransforms;
        [SerializeField] private int maxChange;
        [SerializeField] private float changeDirectionDelayTime;

        // Reference
        private RandomLinePattern _randomLinePattern;
        private RandomShapePattern _randomShapePattern;

        #endregion

        #region Labirin Kata Callbacks
        
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();
            InitializePatternClass();
            SetFirstPosition(movePointTransforms);

            SetPattern();
            InitializePattern();
        }

        protected override void EnemyPatternDirection()
        {
            base.EnemyPatternDirection();
            UpdatePattern();
        }

        private void InitializePatternClass()
        {
            _randomLinePattern = new RandomLinePattern(movePointTransforms, this, EnemyHelper, maxChange, changeDirectionDelayTime);
            _randomShapePattern = new RandomShapePattern(movePointTransforms, this, EnemyHelper, changeDirectionDelayTime);
        }

        // !-- Helper/Utilities
        private void SetPattern()
        {
            switch (patternType)
            {
                case EnemyPatternType.Line:
                    SwitchPattern(_randomLinePattern);
                    break;
                case EnemyPatternType.Shape:
                    SwitchPattern(_randomShapePattern);
                    break;
            }
        }

        #endregion
    }
}