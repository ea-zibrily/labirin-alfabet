using System.Collections;
using UnityEngine;
using TMPro;
using Alphabet.Gameplay.EventHandler;

namespace Alphabet.Gameplay.Controller
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
        private float _latestTime;
        private float _currentTime;

        public bool IsTimerStart
        {
            get => isTimerStart;
            set => isTimerStart = value;
        }

        public float FullTime => _fullTime;
        public float CurrentTime => _currentTime;

        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void OnEnable()
        {
            // Camera
            CameraEventHandler.OnCameraShiftIn += StopTimer;
            CameraEventHandler.OnCameraShiftOut += StartTimer;

            // Game
            GameEventHandler.OnGameStart += StartTimer;
        }
        
        private void OnDisable()
        {
            // Camera
            CameraEventHandler.OnCameraShiftIn -= StopTimer;
            CameraEventHandler.OnCameraShiftOut -= StartTimer;

            // Game
            GameEventHandler.OnGameStart -= StartTimer;
        }
        
        private void Start()
        {
           InitializeTimer();
        }
        
        private void Update()
        {
            CountdownTimer();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        public void InitializeTimer()
        {
            _fullTime = amountOfTime + _latestTime;
            _currentTime = _fullTime;

            TimerDisplay(_currentTime);
        }

        // !-- Core Functionality
        private void CountdownTimer()
        {
            if (!IsTimerStart) return;

            _currentTime -= Time.deltaTime;
            if (_currentTime < 1)
            {
                _currentTime = 0;
                isTimerStart = false;
                GameEventHandler.GameOverEvent();
            }
            
            TimerDisplay(_currentTime);
        }
        
        // !-- Helper/Utitilies
        public void SetLatestTimer() => _latestTime = _currentTime;
        
        private void StartTimer() => IsTimerStart = true;
        private void StopTimer() => IsTimerStart = false;

        private void TimerDisplay(float time)
        {
            var timeInMinutes = Mathf.FloorToInt(time / 60);
            var timeInSeconds = Mathf.FloorToInt(time % 60);
            
            timerTextUI.text = $"{timeInMinutes:00}:{timeInSeconds:00}";
        }
        
        #endregion
    }
}