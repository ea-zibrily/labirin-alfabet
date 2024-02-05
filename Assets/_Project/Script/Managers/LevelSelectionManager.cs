using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using LabirinKata.Enum;
using UnityEngine.Serialization;

namespace LabirinKata.Managers
{
    public class LevelSelectionManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("UI")] 
        [SerializeField] private Button backButtonUI;
        [SerializeField] private Button exploreButtonUI;
        [SerializeField] private Button collectionButtonUI;
        [SerializeField] private GameObject collectionPanelUI;
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Start()
        {
            InitializeLevelSelection();
        }
        
        #endregion

        #region Labirin Kata Callbacks
        
        // !-- Initialization
        private void InitializeLevelSelection()
        {
            collectionPanelUI.SetActive(false);
            
            exploreButtonUI.onClick.AddListener(OnPlayButton);
            collectionButtonUI.onClick.AddListener(OnCollectionButton);
            backButtonUI.onClick.AddListener(OnBackButton);
        }
        
        // !-- Core Functionality
        private void OnPlayButton()
        {
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.NextLevel);
        }
        
        private void OnCollectionButton()
        {
            collectionPanelUI.SetActive(true);
        }

        private void OnBackButton()
        {
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.MainMenu);
        }
        
        #endregion
    }
}