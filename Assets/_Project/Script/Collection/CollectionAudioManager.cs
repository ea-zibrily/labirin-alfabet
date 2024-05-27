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

        // Reference
        private CollectionManager _collectionManager;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
            var collection = transform.parent.gameObject;
            _collectionManager = collection.GetComponentInChildren<CollectionManager>();
        }

        private void OnEnable()
        {
            // Open and Close Collection
            _collectionManager.OnCollectionOpen += EnableAudio;
            _collectionManager.OnCollectionClose += DisableAudio;

            // Snap
            _collectionManager.SimpleScrollSnap.OnSnappingBegin += DisableAudio;
            _collectionManager.SimpleScrollSnap.OnSnappingBegin += EnableAudio;
        }

        private void OnDisable()
        {
            // Open and Close Collection
            _collectionManager.OnCollectionOpen -= EnableAudio;
            _collectionManager.OnCollectionClose -= DisableAudio;

            // Snap
            _collectionManager.SimpleScrollSnap.OnSnappingBegin -= DisableAudio;
            _collectionManager.SimpleScrollSnap.OnSnappingBegin -= EnableAudio;
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
            
            _audioSource.clip = letterAudio;
            _audioSource.Play();
        }

        public void StopAudio()
        {
            _audioSource.Stop();
            _audioSource.clip = null;
        }

        // !-- Helper/Utilities
        private void EnableAudio()
        {
            _audioSource.clip = null;
            _audioSource.enabled = true;
        }

        private void DisableAudio()
        {
            if (IsAudioPlaying())
            {
                StopAudio();
            }
            _audioSource.enabled = false;
        }

        public bool IsAudioPlaying()
        {
            return _audioSource.isPlaying;
        }
        
        #endregion
    }
}