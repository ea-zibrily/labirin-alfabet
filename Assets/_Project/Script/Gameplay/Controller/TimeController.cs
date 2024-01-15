using System;
using UnityEngine;
using TMPro;
using CariHuruf.Entities.Item;
using CariHuruf.Gameplay.EventHandler;

namespace CariHuruf.Gameplay.Controller
{
    public class TimeController : MonoBehaviour
    {
        #region Variable
        
        [Header("Timer")]
        [Tooltip("Isi variable ini dengan total waktu dalam jumlah detik")]
        [SerializeField] private float amountOfTime;
        [SerializeField] private TextMeshProUGUI timerTextUI;
        [SerializeField] private bool isTimerStart;

        private float _fullTime;
        private float _currentTime;
        
        public float FullTime { get { return _fullTime; } } 
        public float CurrentTime { get {return _currentTime;} }
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void Start()
        {
           InitializeTimer();
        }
        
        private void Update()
        {
            CountdownTimer();
        }
        
        #endregion
        
        #region CariHuruf Callbacks
        
        //-- Initialization
        private void InitializeTimer()
        {
            _fullTime = amountOfTime + GameManager.Instance.GetLatestTime();
            _currentTime = _fullTime;
            Debug.LogWarning(_currentTime);
            
            timerTextUI.text = "00:00";
            isTimerStart = true;
        }
        
        //-- Core Functionality
        private void CountdownTimer()
        {
            if (!isTimerStart) return;

            _currentTime -= Time.deltaTime;
            TimerDisplay(_currentTime);
            
            if (_currentTime <= 0)
            {
                _currentTime = 0;
                isTimerStart = false;
                GameEventHandler.GameOverEvent();
            }
        }
        
        private void TimerDisplay(float time)
        {
            var timeInMinutes = Mathf.FloorToInt(time / 60);
            var timeInSeconds = Mathf.FloorToInt(time % 60);
            
            timerTextUI.text = string.Format("{00:00}:{01:00}", timeInMinutes, timeInSeconds);
        }
        
        #endregion
    }
}