using UnityEngine;
using Alphabet.Letter;

namespace Alphabet.Collection
{
    public class CollectionAudio : LetterAudio
    {
        #region Internal Fields

        // Reference
        [SerializeField] private CollectionManager collectionManager;
        
        #endregion

        #region MonoBehaviour Callbacks

        protected override void OnEnable()
        {
            base.OnEnable();

            // Open and Close Collection
            CollectionEventHandler.OnCollectionOpen += EnableAudio;
            CollectionEventHandler.OnCollectionClose += DisableAudio;

            // Snap
            collectionManager.SimpleScrollSnap.OnSnappingBegin += DisableAudio;
            collectionManager.SimpleScrollSnap.OnSnappingBegin += EnableAudio;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // Open and Close Collection
            CollectionEventHandler.OnCollectionOpen -= EnableAudio;
            CollectionEventHandler.OnCollectionClose -= DisableAudio;

            // Snap
            collectionManager.SimpleScrollSnap.OnSnappingBegin -= DisableAudio;
            collectionManager.SimpleScrollSnap.OnSnappingBegin -= EnableAudio;
        }

        #endregion

        #region Methods
        
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