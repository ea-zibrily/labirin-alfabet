using LabirinKata.Enum;
using LabirinKata.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace LabirinKata.UI
{
    public class GameOverController : GameUIBase
    {
        #region Variable
        
        [SerializeField] private Button retryButtonUI;
        
        #endregion

        #region Labirin Kata Callbacks
        
        //-- Initialization
        protected override void InitializeOnStart()
        {
            base.InitializeOnStart();
            retryButtonUI.onClick.AddListener(OnRetryButton);
        }
        
        //-- Core Functionality
        private void OnRetryButton()
        {
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.CurrentLevel);
        }

        #endregion
    }
}