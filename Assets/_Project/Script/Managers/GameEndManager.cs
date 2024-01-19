using System;
using UnityEngine;
using UnityEngine.UI;

namespace LabirinKata.Managers
{
    public class GameEndManager : MonoBehaviour
    {
        #region Variable

        [Header("UI")] 
        [SerializeField] private Button homeButtonUI;
        [SerializeField] private Button nextButtonUI;
        [SerializeField] private Button retryButtonUI;
        
        [Header("Scoring")] 
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
        
        
        
        //-- Core Functionality
        
        
        //-- Helpers/Utilites
        private void ActivateStarUI(int starCount)
        {
            if (starCount >= scoringObjectUI.Length)
            {
                Debug.LogError("start count lebih banyak dari star rating ui!");
                return;
            }

            for (var i = 0; i < starCount; i++)
            {
                scoringObjectUI[i].SetActive(true);
            }
        }

        #endregion
    }
}