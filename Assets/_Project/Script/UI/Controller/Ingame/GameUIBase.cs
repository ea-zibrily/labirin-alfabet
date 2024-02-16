using UnityEngine;
using UnityEngine.UI;
using LabirinKata.Enum;
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

        private void Awake()
        {
            InitializeOnAwake();
        }

        private void Start()
        {
            InitializeOnStart();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        protected virtual void InitializeOnAwake() { }

        protected virtual void InitializeOnStart()
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