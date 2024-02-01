using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using LabirinKata.Enum;
using UnityEngine.Serialization;

namespace LabirinKata.Managers
{
    public class LevelSelectionManager : MonoBehaviour
    {
        #region Variable
        
        [Header("UI")] 
        [SerializeField] private Button levelButtonUI;
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
        
        //-- Initialization
        private void InitializeLevelSelection()
        {
            collectionPanelUI.SetActive(false);
            
            levelButtonUI.onClick.AddListener(OnPlayButton);
            collectionButtonUI.onClick.AddListener(OnCollectionButton);
        }
        
        
        //-- Core Functionality
        private void OnPlayButton()
        {
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.NextLevel);
        }
        
        private void OnCollectionButton()
        {
            collectionPanelUI.SetActive(true);
        }
        
        #endregion
    }
}