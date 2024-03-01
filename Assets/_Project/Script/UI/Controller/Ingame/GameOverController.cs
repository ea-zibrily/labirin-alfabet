using UnityEngine;
using UnityEngine.UI;
using Alphabet.Enum;
using Alphabet.Managers;

namespace Alphabet.UI
{
    public class GameOverController : GameUIBase
    {
        #region Fields & Properties
        
        [SerializeField] private Button retryButtonUI;
        
        #endregion

        #region Labirin Kata Callbacks
        
        // !-- Initialization
        protected override void InitializeOnStart()
        {
            base.InitializeOnStart();
            retryButtonUI.onClick.AddListener(OnRetryButton);
        }
        
        // !-- Core Functionality
        private void OnRetryButton()
        {
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.CurrentLevel);
        }

        #endregion
    }
}