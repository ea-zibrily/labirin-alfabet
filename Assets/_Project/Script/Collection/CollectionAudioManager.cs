using System;
using UnityEngine;
using Alphabet.Item;

namespace Alphabet.Collection
{
    public class CollectionAudioManager : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField] private LetterContainer letterContainer;
        private AudioSource _audioSource;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        #endregion
        
        #region Methods
        
        // !-- Core Functionality
        public void PlayAudio(int id)
        {
            var letterData = letterContainer.GetLetterDataById(id);
            var letterAudio = letterData.LetterAudio;
            if (letterAudio == null)
            {
                Debug.LogError("audionya gada kang");
                return;                
            }

            Debug.Log($"gas sound letter {letterData.LetterName}");
            _audioSource.clip = letterAudio;
            _audioSource.Play();
        }

        public void StopAudio()
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