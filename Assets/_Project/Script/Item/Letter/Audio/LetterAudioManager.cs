using System;
using System.Collections.Generic;
using UnityEngine;
using Alphabet.Letter;
using Alphabet.Collection;

namespace Alphabet.Letter
{
    public class LetterAudioManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Audio Stats")]
        [SerializeField] [Range(0f, 1.0f)] private float audioVolume;
        [SerializeField] [Range(0f, 1.0f)] private float audioPitch = 1f;

        // Event Handler
        public static event Action<int> OnPlayAudio;
        public static event Action OnStopAudio;

        [Header("Reference")]
        [SerializeField] private LetterContainer letterContainer;
        private AudioSource _audioSource;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        private void OnEnable()
        {
            OnPlayAudio += PlayAudio;
            OnStopAudio += StopAudio;
        }

        private void OnDisable()
        {
            OnPlayAudio -= PlayAudio;
            OnStopAudio -= StopAudio;
        }

        private void Start()
        {
            // Init audio source
            _audioSource.volume = audioVolume;
            _audioSource.pitch = audioPitch;
            _audioSource.loop = false;
        }

        #endregion
        
        #region Methods
        
        // !-- Core Functionality
        public static void PlayAudioEvent(int id) => OnPlayAudio?.Invoke(id);
        public static void StopAudioEvent() => OnStopAudio?.Invoke();

        private void PlayAudio(int id)
        {
            var letterData = letterContainer.GetLetterDataById(id);
            var letterAudio = letterData.LetterAudio;
            if (letterAudio == null)
            {
                Debug.LogError("audionya gada kang");
                return;                
            }
            
            _audioSource.clip = letterAudio;
            _audioSource.Play();
        }

        private void StopAudio()
        {
            _audioSource.Stop();
            _audioSource.clip = null;
        }

        // !-- Helper/Utilities
        public bool IsAudioPlaying()
        {
            return _audioSource.isPlaying;
        }
        
        #endregion
    }
}