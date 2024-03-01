using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using Alphabet.Item;
using Alphabet.Database;
using System.Linq;

namespace Alphabet.Collection
{
    public class CollectionManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Collection")] 
        [SerializeField] private RectTransform collectionContentUI;
        private GameObject[] _collectionObjectUI;

        // Const Variable
        private const int FIRST_COLLECTION_GROUP = 3;
        
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
            InitializeObject();
            InitializeCollection();
        }
        
        #endregion

        #region Methods

        // !-- Initialization
        private void InitializeObject()
        {
            var contentCount = collectionContentUI.childCount;
            _collectionObjectUI = new GameObject[contentCount];

            for (var i = 0; i < contentCount; i++)
            {
                var contentObject = collectionContentUI.GetChild(i).gameObject;
                _collectionObjectUI[i] = contentObject;
            }
        }
        
        private void InitializeCollection()
        {
            if (_collectionObjectUI.Length < GameDatabase.LETTER_COUNT)
            {
                Debug.LogError("letter object kurang brok");
                return;
            }

            for (var i = 0; i < _collectionObjectUI.Length; i++)
            {
                var collectionId = i + 1;
                var collectionObject = _collectionObjectUI[i].transform.GetChild(0).gameObject;
                
                InitializeElement(collectionId, collectionObject);
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

            simpleScrollSnap.Setup();
            ActivateContent();
            collectionPanelUI.SetActive(false);
        }

        private void ActivateContent()
        {
            for (var i = 0; i < FIRST_COLLECTION_GROUP; i++)
            {
                var contentObject = collectionContentUI.GetChild(i).gameObject;
                contentObject.SetActive(true);
            }
        }

        #endregion
        
    }
}