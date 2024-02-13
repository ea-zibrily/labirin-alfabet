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

        [Space]
        [SerializeField] private Button collectionButtonUI;
        [SerializeField] private GameObject collectionPanelUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            InitializeButton();
        }

        #endregion

        #region Labirin Kata Callbacks

        // !-- Initialization
        private void InitializeButton()
        {
            collectionPanelUI.SetActive(false);
            
            playButtonUI.onClick.AddListener(OnPlayButton);
            collectionButtonUI.onClick.AddListener(OnCollectionButton);
        }

        // !-- Core Functionality
        private void OnPlayButton()
        {
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.LevelSelectionMenu);
        }

        private void OnCollectionButton()
        {
            collectionPanelUI.SetActive(true);
        }

        #endregion
    }
}