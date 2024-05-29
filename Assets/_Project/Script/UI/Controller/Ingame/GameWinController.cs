using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Alphabet.Enum;
using Alphabet.Managers;
using Alphabet.Gameplay.EventHandler;

namespace Alphabet.UI
{
    public class GameWinController : GameUIBase
    {
        #region Fields & Properties
        
        [SerializeField] private Button nextButtonUI;
        [SerializeField] private ParticleSystem confettiVfx;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            GameEventHandler.OnGameWin += () => StartCoroutine(ConfettiHandlerRotuine());
        }

        private void OnDisable()
        {
            GameEventHandler.OnGameWin += () => StartCoroutine(ConfettiHandlerRotuine());
        }

        #endregion
        
        #region Methods
        
        protected override void InitializeOnStart()
        {
            base.InitializeOnStart();
            nextButtonUI.onClick.AddListener(OnNextButton);
        }
        
        private void OnNextButton()
        {
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.NextLevel);
        }

        private IEnumerator ConfettiHandlerRotuine()
        {
            var duration = confettiVfx.main.duration;

            confettiVfx.Play();
            yield return new WaitForSeconds(duration);
            confettiVfx.Stop();

        }
        
        #endregion
        
    }
}