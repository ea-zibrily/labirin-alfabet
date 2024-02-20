using System;
using UnityEngine;
using UnityEngine.UI;
using LabirinKata.Enum;
using DanielLochner.Assets.SimpleScrollSnap;

namespace LabirinKata.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("UI")] 
        [SerializeField] private GameObject mainMenuPanelUI;
        [SerializeField] private GameObject collectionPanelUI;
        [SerializeField] private GameObject selectStagePanelUI;

        [Space]
        [SerializeField] private Button playButtonUI;
        [SerializeField] private Button collectionButtonUI;

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
            // SceneTransitionManager.Instance.LoadSelectedScene(SceneState.LevelSelectionMenu);
            selectStagePanelUI.SetActive(true);
        }

        private void OnCollectionButton()
        {
            collectionPanelUI.SetActive(true);
            mainMenuPanelUI.SetActive(false);
        }

        #endregion
    }
}