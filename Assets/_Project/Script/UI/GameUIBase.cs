using System;
using LabirinKata.Enum;
using UnityEngine;
using UnityEngine.UI;
using LabirinKata.Managers;

namespace LabirinKata.UI
{
    public class GameUIBase : MonoBehaviour
    {
        #region Base Variable

        [Header("UI")] 
        [SerializeField] private Button homeButtonUI;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            InitializeComponent();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks

        protected virtual void InitializeComponent()
        {
            homeButtonUI.onClick.AddListener(OnHomeButton);
        }
        
        private void OnHomeButton()
        {
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.MainMenu);
        }

        #endregion
        
    }
}