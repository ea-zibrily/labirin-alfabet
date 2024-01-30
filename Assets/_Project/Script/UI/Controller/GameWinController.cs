using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using LabirinKata.Enum;
using LabirinKata.Managers;

namespace LabirinKata.UI
{
    public class GameWinController : GameUIBase
    {
        #region Variable
        
        [SerializeField] private Button nextButtonUI;
        [SerializeField] private GameObject developmentPanelUI;
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        //-- Initialization
        protected override void InitializeOnStart()
        {
            base.InitializeOnStart();
            nextButtonUI.onClick.AddListener(OnNextButton);
        }
        
        //-- Core Functionality
        private void OnNextButton()
        {
            // SceneTransitionManager.Instance.LoadSelectedScene(SceneState.NextLevel);
            StartCoroutine(DevelopmentRoutine());
        }
        
        private IEnumerator DevelopmentRoutine()
        {
            developmentPanelUI.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            developmentPanelUI.SetActive(false);
        }
        
        #endregion
        
    }
}