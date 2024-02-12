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
        private Button _buttonUI;
        
        [Header("Reference")] 
        private CollectionAudioManager _collectionAudioManager;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _buttonUI = GetComponent<Button>();
            var collectionObject = GameObject.FindGameObjectWithTag("Collection");
            _collectionAudioManager = collectionObject.GetComponentInChildren<CollectionAudioManager>();
        }
        
        private void Start()
        {
            InitializeButton();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        private void InitializeButton()
        {
            _buttonUI.onClick.AddListener(ClickObject);
        }
        
        // !-- Core Functionality
        /*
         * TODO: bikin logic buat efek letter jika ditekan
         * Efek list:
         * 1. Animasi scaling
         * 2. Sound effect VO sesuai nama letter
         */
        private void ClickObject()
        {
            _collectionAudioManager.PlayCollectionAudio(letterName);
        }

        #endregion
        
    }
}