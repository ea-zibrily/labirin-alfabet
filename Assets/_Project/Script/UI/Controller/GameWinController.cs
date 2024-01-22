using LabirinKata.Enum;
using LabirinKata.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace LabirinKata.UI
{
    public class GameWinController : GameUIBase
    {
        #region Variable
        
        [SerializeField] private Button nextButtonUI;
        [SerializeField] private GameObject[] scoringObjectUI;
        
        [Header("Reference")] 
        private ScoreManager _scoreManager;
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        }
        
        #endregion

        #region Labirin Kata Callbacks
        
        //-- Initialization
        protected override void InitializeOnStart()
        {
            base.InitializeOnStart();
            nextButtonUI.onClick.AddListener(OnNextButton);
            SetScoringUI();
        }
        
        //-- Core Functionality
        private void SetScoringUI()
        {
            var levelScore = _scoreManager.ScoreGetCount;
            DeactivateScoreUI();
            ActivateScoreUI(levelScore);
        }
        
        private void OnNextButton()
        {
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.NextLevel);
        }
        
        //-- Helpers/Utilites
        private void DeactivateScoreUI()
        {
            foreach (var scoreUI in scoringObjectUI)
            {
                scoreUI.SetActive(false);
            }
        }

        // NOTE: Pakai method ini misal UI scorenya paket
        private void ActivateScoreUI(int score)
        {
            if (score >= scoringObjectUI.Length)
            {
                Debug.LogError("score lebih banyak dari score object ui!");
                return;
            }
            
            var scoreIndex = score - 1;
            scoringObjectUI[scoreIndex].SetActive(true);
            
            LeanTween.scale(scoringObjectUI[scoreIndex], new Vector3(1f, 1f, 1f),2f).setDelay(0.2f)
                .setEase(LeanTweenType.easeOutElastic);
        }
        
        // NOTE: Pakai method ini misal UI scorenya satuan (bintang satuan, tidak satu paket)
        private void ActivateSingleScoreUI(int score)
        {
            if (score >= scoringObjectUI.Length)
            {
                Debug.LogError("score lebih banyak dari score object ui!");
                return;
            }
            
            for (var i = 0; i < score; i++)
            {
                scoringObjectUI[i].SetActive(true);
            }
        }
        
        #endregion
        
    }
}