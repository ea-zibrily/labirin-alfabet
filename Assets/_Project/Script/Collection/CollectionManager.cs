using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using LabirinKata.Item;
using LabirinKata.Database;

namespace LabirinKata.Collection
{
    public class CollectionManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Collection")] 
        [SerializeField] private GameObject[] collectionParentUI;

        private readonly List<GameObject> _collectionObjectUI;
        
        [Header("UI")] 
        [SerializeField] private GameObject mainMenuPanelUI;
        [SerializeField] private GameObject collectionPanelUI;
        [SerializeField] private Button closeButtonUI;

        [Header("Reference")]
        [SerializeField] private LetterContainer letterContainer;
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
            InitializeComponent();
        }
        
        #endregion

        #region Labirin Kata Callbacks

        // !-- Initialization
        private void InitializeCollection()
        {
            foreach (var collection in collectionParentUI)
            {
                var collectionObject = collection.transform.GetChild(0).gameObject;
                _collectionObjectUI.Add(collectionObject);
            }
        }

        private void InitializeComponent()
        {
            if (_collectionObjectUI.Count < GameDatabase.LETTER_COUNT)
            {
                Debug.LogError("letter object kurang brok");
                return;
            }

            for (var i = 0; i < _collectionObjectUI.Count; i++)
            {
                var collectionId = i + 1;
                InitializeElement(collectionId, _collectionObjectUI[i]);
            }

            closeButtonUI.onClick.AddListener(CloseCollection);
        }
        
        private void InitializeElement(int id, GameObject collection)
        {
            var letterData = letterContainer.GetLetterDataById(id);
            var collectionController = collection.GetComponent<CollectionController>();
            var button = collection.GetComponent<Button>();
            var fillImage = collection.transform.GetChild(0).gameObject;

            collectionController.InitializeData(letterData);
            button.interactable = GameDatabase.Instance.LoadLetterConditions(id);
            fillImage.SetActive(button.interactable);
        }

        // !-- Core Functionality
        private void CloseCollection()
        {
            _collectionAudioManager.StopAudio();
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