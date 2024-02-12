using System;
using UnityEngine;
using LabirinKata.Enum;

namespace LabirinKata.Collection
{
    public class CollectionAudioManager : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField] private CollectionSound[] collectionSounds;
        private AudioSource _audioSource;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        #endregion
        
        #region Labirin Kata Callbacks
        
        public void PlayCollectionAudio(string letterName)
        {
            var sound = Array.Find(collectionSounds, sound => sound.LetterName == letterName);
            
            if (sound == null)
            {
                Debug.Log("soundny gada kang");
                return;
            }

            Debug.Log($"gas sound letter {sound.LetterName}");
            _audioSource.clip = sound.LetterSound;
            _audioSource.Play();
        }
        
        #endregion
    }
}