using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Alphabet.Enum;
using Alphabet.Managers;

namespace Alphabet.UI
{
    public class GameWinController : GameUIBase
    {
        #region Fields & Properties
        
        [SerializeField] private Button nextButtonUI;
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        protected override void InitializeOnStart()
        {
            base.InitializeOnStart();
            nextButtonUI.onClick.AddListener(OnNextButton);
        }
        
        // !-- Core Functionality
        private void OnNextButton()
        {
            // SceneTransitionManager.Instance.LoadSelectedScene(SceneState.NextLevel);
        }
        
        #endregion
        
    }
}