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

        #endregion
        
        #region Methods
        
        protected override void InitializeOnStart()
        {
            base.InitializeOnStart();
            nextButtonUI.onClick.AddListener(OnNextButton);
        }
        
        private void OnNextButton()
        {
            FindObjectOfType<AudioManager>().PlayAudio(Musics.ButtonSfx);
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.NextLevel);
        }
        
        #endregion
        
    }
}