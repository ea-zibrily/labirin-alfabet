using UnityEngine;
using UnityEngine.UI;
using Alphabet.Enum;
using Alphabet.Managers;

namespace Alphabet.UI
{
    public class GameUIBase : MonoBehaviour
    {
        #region Base Variable

        [Header("UI")] 
        [SerializeField] private Button homeButtonUI;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            InitializeOnAwake();
        }

        private void Start()
        {
            InitializeOnStart();
        }
        
        #endregion
        
        #region Methods
        
        protected virtual void InitializeOnAwake() { }

        protected virtual void InitializeOnStart()
        {
            homeButtonUI.onClick.AddListener(OnHomeButton);
        }
        
        private void OnHomeButton()
        {
            FindObjectOfType<AudioManager>().PlayAudio(Musics.ButtonSfx);
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.MainMenu);
        }

        #endregion
        
    }
}