using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using KevinCastejon.MoreAttributes;
using DanielLochner.Assets.SimpleScrollSnap;
using LabirinKata.Database;

namespace LabirinKata.Collection
{
    public class CollectionManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("UI")] 
        [SerializeField] private GameObject[] letterObjectUI;
        [SerializeField] private GameObject mainMenuPanelUI;
        [SerializeField] private GameObject collectionPanelUI;
        [SerializeField] private Button closeButtonUI;

        [Header("Reference")]
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;
        private CollectionAudioManager _collectionAudioManager;

        public SimpleScrollSnap SimpleScrollSnap => simpleScrollSnap;
        
        #endregion
        
        #region MonoBehaviour Callbacks

        private void Awake() 
        {
            var collectionObject = GameObject.FindGameObjectWithTag("Collection");
            _collectionAudioManager = collectionObject.GetComponentInChildren<CollectionAudioManager>();
        }
        
        private void Start()
        {
            InitializeCollection();
            SetUnlockedCollection();
        }
        
        #endregion

        #region Labirin Kata Callbacks

        // !-- Initialization
        private void InitializeCollection()
        {
            closeButtonUI.onClick.AddListener(CloseCollection);
        }
        
        // !-- Core Functionality
        private void SetUnlockedCollection()
        {
            for (var i = 0; i < letterObjectUI.Length; i++)
            {
                var letterId = i + 1;
                var isLetterUnlock = GameDatabase.Instance.LoadLetterConditions(letterId);
                
                letterObjectUI[i].GetComponent<Button>().interactable = isLetterUnlock;
                letterObjectUI[i].transform.GetChild(0).gameObject.SetActive(isLetterUnlock);
                Debug.Log($"{letterObjectUI[i]} is {isLetterUnlock}");
            }
        }
        
        private void CloseCollection()
        {
            _collectionAudioManager.StopCollectionAudio();
            mainMenuPanelUI.SetActive(true);

            ReSetupScrollSnap();
            collectionPanelUI.SetActive(false);
        }

        private void ReSetupScrollSnap()
        {
            if (simpleScrollSnap.ValidConfig)
            {
                simpleScrollSnap.Setup();
            }
            else
            {
                throw new Exception("Invalid configuration.");
            }
        }

        #endregion
        
    }
}