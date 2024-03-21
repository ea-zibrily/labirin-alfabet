using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alphabet.DesignPattern.Singleton;

namespace Alphabet.Managers
{
    public class FrameRateManager : MonoSingleton<FrameRateManager>
    {
        #region Fields & Properties
        
        [Header("Settings")]
        [Range(0, 120)] [SerializeField] private int targetFrameRate = 30;
        [SerializeField] private bool isFixedFrameRate = true;
        
        #endregion

        #region MonoBehaviour Callbacks

        protected override void Awake() 
        {
            if (!isFixedFrameRate) return;
            
            Application.targetFrameRate = targetFrameRate;
            Debug.Log(Application.targetFrameRate);
        }

        #endregion
    }
}
