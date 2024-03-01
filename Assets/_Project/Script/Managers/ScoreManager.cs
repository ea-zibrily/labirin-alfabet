using UnityEngine;
using Alphabet.Gameplay.Controller;

namespace Alphabet.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        #region Variable

        [Header("Score")] 
        private int _scoreGetCount;
        public int ScoreGetCount => _scoreGetCount;

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
            InitializeScore();
        }

        #endregion

        #region Labirin Kata Callbacks
        
        //-- Initialization
        private void InitializeScore()
        {
            _scoreGetCount = 0;
        }
        
        //-- Core Functionality
        public void RateLevelScore()
        {
            var currentTime = _timeController.CurrentTime;
            var quarterTime = _timeController.FullTime * 0.25f;
            var halfTime = _timeController.FullTime * 0.5f;
            
            var starCount = currentTime switch
            {
                var value when value < quarterTime => 1,
                var value when value < halfTime => 2,
                _ => 3
            };
            
            Debug.LogWarning($"current: {currentTime} - get {starCount} star! " +
                             $"{(starCount switch { 1 => "walawe", 2 => "jos", _ => "mantap bozqku" })}");
            
            _scoreGetCount = starCount;
        }
        
        #endregion
    }
}