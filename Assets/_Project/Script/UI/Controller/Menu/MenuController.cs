using System;
using Alphabet.Collection;
using UnityEngine;
using UnityEngine.UI;

namespace Alphabet.UI
{
    public class MenuController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("UI")] 
        [SerializeField] private GameObject mainMenuPanelUI;
        [SerializeField] private GameObject collectionPanelUI;
        [SerializeField] private GameObject selectStagePanelUI;

        [Space]
        [SerializeField] private Button playButtonUI;
        [SerializeField] private Button collectionButtonUI;

        [Header("Reference")]
        private CollectionManager _collectionManager;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            var collectionObject = GameObject.FindGameObjectWithTag("Collection");
            _collectionManager = collectionObject.GetComponentInChildren<CollectionManager>();
        }

        private void Start()
        {
            InitializeButton();
        }

        #endregion
        
        #region Methods

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
            selectStagePanelUI.SetActive(true);
        }

        private void OnCollectionButton()
        {
            collectionPanelUI.SetActive(true);
            mainMenuPanelUI.SetActive(false);
            _collectionManager.OnCollectionOpenEvent();
        }

        #endregion
    }
}