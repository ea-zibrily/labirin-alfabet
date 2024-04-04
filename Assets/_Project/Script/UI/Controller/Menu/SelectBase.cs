using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Alphabet.UI
{
    public class SelectBase : MonoBehaviour
    {
        #region Fields & Property

        [Header("Button")]
        [SerializeField] private Button exploreButtonUI;
        [SerializeField] private Button closeButtonUI;

        public Button ExploreButtonUI => exploreButtonUI;
        protected Button CloseButtonUI => closeButtonUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            InitialiazeOnStart();
        }

        #endregion

        #region Methods

        // !-- Initialization
        protected virtual void InitialiazeOnStart()
        {
            exploreButtonUI.onClick.AddListener(OnClickExplore);
            closeButtonUI.onClick.AddListener(OnClickClose);
        }

        // !-- Core Initialization
        protected virtual void OnClickExplore() { }
        protected virtual void OnClickClose() { }

        #endregion
    }
}
