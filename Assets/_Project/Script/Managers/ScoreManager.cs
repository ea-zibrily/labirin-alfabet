using LabirinKata.Gameplay.Controller;
using UnityEngine;

namespace LabirinKata.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        #region Variable

        [Header("Scoring")] 
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
            InitializeStarRating();
        }

        #endregion

        #region CariHuruf Callbacks

        //-- Initialization
        private void InitializeStarRating()
        {
            if (starRatingUI == null)
            {
                Debug.LogError("star ui null brok!");
                return;
            }
            
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
            var starCount = 0;
            
            if (currentTime < quarterTime)
            {
                starCount = 1;
                Debug.LogWarning($"current: {currentTime} < quarter {quarterTime}");
                Debug.LogWarning($"get {starCount} star! walawe");
            }
            else if (currentTime < halfTime)
            {
                starCount = 2;
                Debug.LogWarning($"current: {currentTime} < halftime {halfTime}");
                Debug.LogWarning($"get {starCount} star! jos");
            }
            else if (currentTime >= halfTime)
            {
                starCount = 3;
                Debug.LogWarning($"current: {currentTime} >= halftime {halfTime}");
                Debug.LogWarning($"get {starCount} star! mantap bozqku");
            }
            
            // ActivateStarUI(starCount);
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