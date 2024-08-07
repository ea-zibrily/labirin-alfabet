using System;
using UnityEngine;
using Alphabet.Letter;

namespace Alphabet.Collection
{
    public class CollectionAudioManager : LetterAudioManager
    {
        #region Internal Fields

        private CollectionManager _collectionManager;
        
        #endregion

        #region MonoBehaviour Callbacks

        protected override void OnEnable()
        {
            base.OnEnable();

            // Open and Close Collection
            _collectionManager.OnCollectionOpen += EnableAudio;
            _collectionManager.OnCollectionClose += DisableAudio;

            // Snap
            _collectionManager.SimpleScrollSnap.OnSnappingBegin += DisableAudio;
            _collectionManager.SimpleScrollSnap.OnSnappingBegin += EnableAudio;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // Open and Close Collection
            _collectionManager.OnCollectionOpen -= EnableAudio;
            _collectionManager.OnCollectionClose -= DisableAudio;

            // Snap
            _collectionManager.SimpleScrollSnap.OnSnappingBegin -= DisableAudio;
            _collectionManager.SimpleScrollSnap.OnSnappingBegin -= EnableAudio;
        }

        #endregion

        #region Methods

        protected override void InitOnAwake()
        {
            base.InitOnAwake();
            var collection = transform.parent.gameObject;
            _collectionManager = collection.GetComponentInChildren<CollectionManager>();
        }
        
        private void EnableAudio()
        {
            _audioSource.clip = null;
            _audioSource.enabled = true;
        }

        private void DisableAudio()
        {
            if (_audioSource.isPlaying)
            {
                StopAudioEvent();
            }
            _audioSource.enabled = false;
        }
        
        #endregion
    }
}