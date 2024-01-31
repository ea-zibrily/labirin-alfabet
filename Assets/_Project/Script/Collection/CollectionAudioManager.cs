using System;
using UnityEngine;
using LabirinKata.Enum;
using UnityEngine.Serialization;

namespace LabirinKata.Collection
{
    public class CollectionAudioManager : MonoBehaviour
    {
        #region Variable

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
        
        public void PlayCollectionAudio(Letter letter)
        {
            var sound = Array.Find(collectionSounds, sound => sound.LetterName == letter.ToString());
            
            if (sound == null)
            {
                Debug.Log("soundny gada kang");
                return;
            }

            _audioSource.clip = sound.LetterSound;
            _audioSource.Play();

        }
        
        #endregion
    }
}