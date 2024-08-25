using UnityEngine;
using UnityEngine.UI;
using Alphabet.Enum;
using Alphabet.Managers;
using Alphabet.Collection;

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

        // Reference
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
            collectionPanelUI.SetActive(false);
            InitializeButton();
        }

        #endregion
        
        #region Methods
        
        // !- Initialize
        private void InitializeButton()
        {
            playButtonUI.onClick.AddListener(OnPlayButton);
            collectionButtonUI.onClick.AddListener(OnCollectionButton);
            backButtonUI.onClick.AddListener(OnBackButton);
        }
        
        // !- Core
        public void OnPlayButton()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            selectStagePanelUI.SetActive(true);
        }

        private void OnCollectionButton()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);

            collectionPanelUI.SetActive(true);
            mainMenuPanelUI.SetActive(false);
            CollectionEventHandler.CollectionOpenEvent();
        }

        // TODO: Drop logic buat kembali ke iota kids
        private void OnBackButton()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
        }

        #endregion
    }
}