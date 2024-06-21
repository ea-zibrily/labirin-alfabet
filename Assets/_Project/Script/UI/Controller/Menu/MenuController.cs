using System;
using Alphabet.Collection;
using UnityEngine;
using UnityEngine.UI;
using Alphabet.Enum;
using Alphabet.Managers;

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
        [SerializeField] private Button backButtonUI;

        public Button PlayButtonUI => playButtonUI;

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
            backButtonUI.onClick.AddListener(OnBackButton);
        }
        
        // !-- Core Functionality
        public void OnPlayButton()
        {
            FindObjectOfType<AudioManager>().PlayAudio(Musics.ButtonSfx);
            selectStagePanelUI.SetActive(true);
        }

        private void OnCollectionButton()
        {
            FindObjectOfType<AudioManager>().PlayAudio(Musics.ButtonSfx);

            collectionPanelUI.SetActive(true);
            mainMenuPanelUI.SetActive(false);
            _collectionManager.OnCollectionOpenEvent();
        }

        // TODO: Drop logic buat kembali ke iota kids
        private void OnBackButton()
        {
            FindObjectOfType<AudioManager>().PlayAudio(Musics.ButtonSfx);
        }

        #endregion
    }
}