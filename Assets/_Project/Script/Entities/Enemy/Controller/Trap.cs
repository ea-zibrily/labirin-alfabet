using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alphabet.Enum;

namespace Alphabet.Entities.Enemy
{
    [AddComponentMenu("Alphabet/Entities/Enemy/Trap")]
    public class Trap : EnemyBase
    {
        #region Fields & Property
        
        [Header("Finder")]
        [SerializeField] private EnemyPatternType patternType;
        [SerializeField] private Transform[] movePointTransforms;

        [Header("Reference")]
        private LinePattern _linePattern;
        private ShapePattern _shapePattern;

        #endregion

        #region Labirin Kata Callbacks

        // !-- Initialization
        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();
            InitializePatternClass();
            SetFirstPosition(movePointTransforms);

            SetPattern();
            InitializePattern();
        }

        private void InitializePatternClass()
        {
            _linePattern = new LinePattern(movePointTransforms, this, 0, isWanderer: false);
            _shapePattern = new ShapePattern(movePointTransforms, this, 0, isWanderer: false);
        }

        // !-- Core Functioanlity
        protected override void EnemyPatternDirection()
        {
            base.EnemyPatternDirection();
            UpdatePattern();
        }

        // !-- Helper/Utilities
        private void SetPattern()
        {
            switch (patternType)
            {
                case EnemyPatternType.Line:
                    SwitchPattern(_linePattern);
                    break;
                case EnemyPatternType.Shape:
                    SwitchPattern(_shapePattern);
                    break;
            }
        }

        #endregion
    }
}