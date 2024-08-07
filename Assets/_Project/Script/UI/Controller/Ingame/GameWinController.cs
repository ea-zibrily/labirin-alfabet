using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Alphabet.Enum;
using Alphabet.Stage;
using Alphabet.Managers;

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
            
            if (StageManager.Instance.CurrentStage == StageName.Kuil_Litera)
                SceneTransitionManager.Instance.LoadSelectedScene(SceneState.MainMenu);
            else
                SceneTransitionManager.Instance.LoadSelectedScene(SceneState.NextLevel);
        }
        
        #endregion
        
    }
}