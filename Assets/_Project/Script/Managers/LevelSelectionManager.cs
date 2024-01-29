using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using LabirinKata.Enum;

namespace LabirinKata.Managers
{
    public class LevelSelectionManager : MonoBehaviour
    {
        #region Variable

        [Header("UI")] 
        [SerializeField] private Button levelButtonUI;
        [SerializeField] private Button colletionButtonUI;
        [SerializeField] private GameObject collectionPanelUI;

        [SerializeField] private GameObject developmentPanel;
        
        #endregion
        
        #region MonoBehaviour Callbacks

        private void Start()
        {
            levelButtonUI.onClick.AddListener(OnPlayButton);
            colletionButtonUI.onClick.AddListener(OnCollectionButton);
        }

        #endregion

        #region Labirin Kata Callbacks
        
        //-- Initialization
        private void OnPlayButton()
        {
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.NextLevel);
        }

        private void OnCollectionButton()
        {
            StartCoroutine(DevelopmentRoutine());
        }
        
        //-- Core Functionality
        private IEnumerator DevelopmentRoutine()
        {
            developmentPanel.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            developmentPanel.SetActive(false);
        }
        
        #endregion
    }
}