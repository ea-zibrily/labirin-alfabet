using UnityEngine;
using Alphabet.Pattern.Singleton;

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
            base.Awake();
            if (!isFixedFrameRate) return;
            Application.targetFrameRate = targetFrameRate;
        }

        #endregion
    }
}
