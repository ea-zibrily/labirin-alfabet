using UnityEngine;
using Alphabet.Enum;
using Alphabet.Managers;
using Tsukuyomi.Utilities;
using Alphabet.Tsukuyomi;
using System;
using System.Collections;

namespace Alphabet.Gameplay.Controller
{
    public class AudioController : MonoBehaviour
    {
        #region Variable

        [SerializeField] private Musics musicName;
        private AudioManager _audioManager;

        // Event
        public static event Action<bool, float> OnFadeAudio;

        #endregion
    
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        private void OnEnable()
        {
            OnFadeAudio += FadeAudio;
        }

        private void OnDisable()
        {
            OnFadeAudio -= FadeAudio;
        }
        
        private void Start()
        {
            var fadeDuration = SceneTransitionManager.Instance.FadeDuration;
            FadeAudioEvent(isFadeIn: true, fadeDuration);
        }

        #endregion

        #region Methods

        public static void FadeAudioEvent(bool isFadeIn, float duration = 0.5f)
        {
            OnFadeAudio?.Invoke(isFadeIn, duration);
        }

        private void FadeAudio(bool isFadeIn, float duration)
        {
            var audio = _audioManager.GetAudio(musicName);
            var volume = audio.volume;
            
            if (isFadeIn)
                StartCoroutine(AudioSourceExt.FadeIn(audio.source, duration, volume));
            else
                StartCoroutine(AudioSourceExt.FadeOut(audio.source, duration));
        }

        #endregion
    }
}
