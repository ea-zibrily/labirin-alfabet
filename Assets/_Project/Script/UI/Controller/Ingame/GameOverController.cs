using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Alphabet.Enum;
using Alphabet.Managers;

namespace Alphabet.UI
{
    public class GameOverController : GameUIBase
    {
        #region Struct
        [Serializable]
        public struct LoseData
        {
            [TextArea(0, 5)] 
            public string loseText;
            public Sprite loseImage;
        }
        #endregion

        #region Fields & Properties
        
        // UI
        [SerializeField] private Button retryButtonUI;
        [SerializeField] private TextMeshProUGUI loseTextUI;
        [SerializeField] private Image loseImageUI;

        [Header("Over")]
        [SerializeField] private LoseData[] loseDatas;
        
        #endregion

        #region Methods
        
        // !- Initialize
        protected override void InitializeOnStart()
        {
            base.InitializeOnStart();
            retryButtonUI.onClick.AddListener(OnRetryButton);
        }
        
        // !- Core
        public void SetGameOverInterface(LoseType loseType)
        {
            var typeIndex = loseType switch
            {
                LoseType.Death => 0,
                LoseType.TimeUp => 1,
                _ => 0,
            };

            loseTextUI.text = loseDatas[typeIndex].loseText;
            loseImageUI.sprite = loseDatas[typeIndex].loseImage;
        }

        private void OnRetryButton()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.CurrentLevel);
        }

        #endregion
    }
}