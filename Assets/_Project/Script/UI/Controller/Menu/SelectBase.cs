using UnityEngine;
using UnityEngine.UI;
using Alphabet.Enum;
using Alphabet.Managers;

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
            InitOnStart();
        }

        #endregion

        #region Methods

        // !-- Initialize
        protected virtual void InitOnStart()
        {
            exploreButtonUI.onClick.AddListener(OnClickExplore);
            closeButtonUI.onClick.AddListener(OnClickClose);
        }

        // !- Core
        protected virtual void OnClickExplore() { }
        protected virtual void OnClickClose()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
        }

        #endregion
    }
}
