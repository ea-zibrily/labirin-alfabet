using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alphabet.DesignPattern.Singleton;

namespace Alphabet.Managers
{
    public class FrameRateManager : MonoDDOL<FrameRateManager>
    {
        #region Fields & Properties
        
        [Header("Settings")]
        [Range(0, 120)] [SerializeField] private int targetFrameRate = 30;
        [SerializeField] private bool isFixedFrameRate = true;

        #endregion

        #region Methods

        // !-- Initialization
        public override void InitComponent() 
        {
            if (!isFixedFrameRate) return;
            Application.targetFrameRate = targetFrameRate;
        }

        #endregion
    }
}
