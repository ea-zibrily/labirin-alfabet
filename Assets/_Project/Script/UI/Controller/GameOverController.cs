﻿using UnityEngine;
using UnityEngine.UI;
using LabirinKata.Enum;
using LabirinKata.Managers;

namespace LabirinKata.UI
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