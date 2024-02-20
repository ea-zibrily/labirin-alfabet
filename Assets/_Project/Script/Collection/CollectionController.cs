using System;
using LabirinKata.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace LabirinKata.Collection
{
    public class CollectionController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Settings")] 
        [SerializeField] private string letterName;

        private bool canInteract;
        private Button _buttonUI;
        
        [Header("Reference")] 
        private CollectionManager _collectionManager;
        private CollectionAudioManager _collectionAudioManager;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _buttonUI = GetComponent<Button>();
            var collectionObject = GameObject.FindGameObjectWithTag("Collection");
            _collectionManager = collectionObject.GetComponentInChildren<CollectionManager>();
            _collectionAudioManager = collectionObject.GetComponentInChildren<CollectionAudioManager>();
        }

        private void OnEnable() 
        {
            _collectionManager.SimpleScrollSnap.OnSnappingBegin += StopAudio;
        }

        private void OnDisable() 
        {
            _collectionManager.SimpleScrollSnap.OnSnappingBegin -= StopAudio;
        }
        
        private void Start()
        {
            InitializeCollection();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        private void InitializeCollection()
        {
            canInteract = true;
            _buttonUI.onClick.AddListener(ClickObject);
        }
        
        // !-- Core Functionality
        private void ClickObject()
        {
            if (!canInteract) return;

            canInteract = false;
            LeanTween.scale(gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.5f).
                    setEase(LeanTweenType.easeOutElastic).setOnComplete(() =>
                    {
                        _collectionAudioManager.PlayCollectionAudio(letterName);
                    });
            
            LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.5f).setDelay(1.5f).
                    setEase(LeanTweenType.easeOutElastic).setOnComplete(() =>
                    {
                        canInteract = true;
                    });       
        }

        private void StopAudio()
        {
            if (!_collectionAudioManager.IsAudioPlaying()) return;
            _collectionAudioManager.StopCollectionAudio();
        }

        #endregion
        
    }
}