using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Alphabet.Enum;
using UnityEngine.Serialization;

namespace Alphabet.Managers
{
    public class LevelSelectionManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("UI")] 
        [SerializeField] private Button backButtonUI;
        [SerializeField] private Button exploreButtonUI;

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
            backButtonUI.onClick.AddListener(OnBackButton);
            exploreButtonUI.onClick.AddListener(OnPlayButton);
        }
        
        // !-- Core Functionality
        private void OnPlayButton()
        {
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.NextLevel);
        }
        
        private void OnBackButton()
        {
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.MainMenu);
        }
        
        #endregion
    }
}