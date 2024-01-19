using System;
using LabirinKata.Enum;
using LabirinKata.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            nextButtonUI.onClick.AddListener(OnNextButton);
            SetScoringUI();
        }
        
        //-- Core Functionality
        private void SetScoringUI()
        {
            var getScore = _scoreManager.ScoreGetCount;
            DeactivateScoreUI();
            ActivateScoreUI(getScore);
        }
        
        private void OnNextButton()
        {
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.NextLevel);
        }
        
        //-- Helpers/Utilites
        private void ActivateScoreUI(int score)
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
        
        private void DeactivateScoreUI()
        {
            foreach (var scoreUI in scoringObjectUI)
            {
                scoreUI.SetActive(false);
            }
        }

        #endregion
        
    }
}