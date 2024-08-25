using System;
using UnityEngine;

namespace Alphabet.Letter
{
    public class LetterAudio : MonoBehaviour
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
        protected AudioSource _audioSource;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            InitOnAwake();
        }

        protected virtual void OnEnable()
        {
            OnPlayAudio += PlayAudio;
            OnStopAudio += StopAudio;
        }

        protected virtual void OnDisable()
        {
            OnPlayAudio -= PlayAudio;
            OnStopAudio -= StopAudio;
        }

        private void Start()
        {
            InitOnStart();
        }

        #endregion
        
        #region Methods
        
        protected virtual void InitOnAwake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        protected virtual void InitOnStart()
        {
            // Init audio source
            _audioSource.volume = audioVolume;
            _audioSource.pitch = audioPitch;
            _audioSource.loop = false;
        }

        
        // !- Core 
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
            if (!_audioSource.isPlaying) return;
            _audioSource.Stop();
            _audioSource.clip = null;
        }
        
        #endregion
    }
}