using System;
using UnityEngine;
using UnityEngine.UI;
using LabirinKata.Enum;

namespace LabirinKata.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("UI")] 
        [SerializeField] private Button playButtonUI;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            playButtonUI.onClick.AddListener(OnPlayButton);
        }

        #endregion

        #region Labirin Kata Callbacks

        // !-- Initialization
        private void OnPlayButton()
        {
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.LevelSelectionMenu);
        }

        #endregion
    }
}