using System;
using UnityEngine;
using CariHuruf.Gameplay.Controller;

namespace CariHuruf.Managers
{
    public class StarRatingManager : MonoBehaviour
    {
        #region Variable

        [Header("Rate")] 
        [SerializeField] private GameObject[] starRatingUI;

        [Header("Reference")] 
        private TimeController _timeController;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        }

        private void Start()
        {
            // InitializeStarRating();
        }

        #endregion

        #region CariHuruf Callbacks

        //-- Initialization
        private void InitializeStarRating()
        {
            foreach (var starRate in starRatingUI)
            {
                starRate.SetActive(false);
            }
        }

        //-- Core Functionality
        public void RateStar()
        {
            var currentTime = _timeController.CurrentTime;
            var quarterTime = _timeController.FullTime * 0.25f;
            var halfTime = _timeController.FullTime * 0.5f;
            
            if (currentTime < quarterTime)
            {
                Debug.LogWarning($"current: {currentTime} < quarter {quarterTime}");
                Debug.LogWarning("get 1 star!");
                // ActivateStarUI(1);
            }
            else if (currentTime < halfTime)
            {
                Debug.LogWarning($"current: {currentTime} < halftime {halfTime}");
                Debug.LogWarning("get 2 star!");
               // ActivateStarUI(2);
            }
            else if (currentTime >= halfTime)
            {
                Debug.LogWarning($"current: {currentTime} >= halftime {halfTime}");
                Debug.LogWarning("get 3 star! mantap");
                // ActivateStarUI(3);
            }
        }
        
        //-- Helpers/Utilites
        private void ActivateStarUI(int starCount)
        {
            if (starCount >= starRatingUI.Length)
            {
                Debug.LogError("start count lebih banyak dari star rating ui!");
                return;
            }

            for (var i = 0; i < starCount; i++)
            {
                starRatingUI[i].SetActive(true);
            }
        }
        
        #endregion
    }
}