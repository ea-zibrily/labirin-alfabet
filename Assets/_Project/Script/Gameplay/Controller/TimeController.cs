﻿using System.Collections;
using UnityEngine;
using TMPro;
using LabirinKata.Gameplay.EventHandler;

namespace LabirinKata.Gameplay.Controller
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
        
        private void Start()
        {
           InitializeTimer();
           StartCoroutine(StartTimerRoutine());
        }
        
        private void Update()
        {
            CountdownTimer();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        //-- Initialization
        public void InitializeTimer()
        {
            _fullTime = amountOfTime + _latestTime;
            _currentTime = _fullTime;
            
            TimerDisplay(_currentTime);
        }
        
        private IEnumerator StartTimerRoutine()
        {
            yield return new WaitForSeconds(1.5f);
            IsTimerStart = true;
        }
        
        //-- Core Functionality
        private void CountdownTimer()
        {
            if (!IsTimerStart) return;

            _currentTime -= Time.deltaTime;
            TimerDisplay(_currentTime);
            
            if (_currentTime <= 0)
            {
                _currentTime = 0;
                isTimerStart = false;
                GameEventHandler.GameOverEvent();
            }
        }
        
        //-- Helper/Utitilies
        public void SetLatestTimer()
        {
            _latestTime = _currentTime;
        }
        
        private void TimerDisplay(float time)
        {
            var timeInMinutes = Mathf.FloorToInt(time / 60);
            var timeInSeconds = Mathf.FloorToInt(time % 60);
            
            timerTextUI.text = $"{timeInMinutes:00}:{timeInSeconds:00}";
        }
        
        #endregion
    }
}