﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using Alphabet.Enum;
using Alphabet.Letter;
using Alphabet.Database;
using Alphabet.Managers;

namespace Alphabet.Collection
{
    public class CollectionManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Collection")] 
        [SerializeField] private RectTransform[] collectionContentUI;
        private GameObject[] _collectionObjectUI;

        private int _selectedCollectionId;
        public int SelectedCollectionId => _selectedCollectionId;
        
        // Event
        public event Action OnCollectionOpen;
        public event Action OnCollectionClose;
        
        [Header("UI")] 
        [SerializeField] private GameObject mainMenuPanelUI;
        [SerializeField] private GameObject collectionPanelUI;
        [SerializeField] private Button closeButtonUI;
        
        [Header("Reference")]
        [SerializeField] private LetterContainer letterContainer;
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;

        public SimpleScrollSnap SimpleScrollSnap => simpleScrollSnap;
        
        #endregion
        
        #region MonoBehaviour Callbacks

        private void Start()
        {
            InitializeObject();
            InitializeCollection();
        }
        
        #endregion

        #region Events

        public void OnCollectionOpenEvent() => OnCollectionOpen?.Invoke();
        public void OnCollectionCloseEvent() => OnCollectionClose?.Invoke();

        #endregion

        #region Methods

        // !-- Initialization
        private void InitializeObject()
        {
            _collectionObjectUI = new GameObject[GameDatabase.LETTER_COUNT];

            var i = 0;
            foreach (RectTransform childContent in collectionContentUI)
            {
                foreach (RectTransform grandChildContent in childContent)
                {
                    foreach (RectTransform greatGrandChildContent in grandChildContent)
                    {
                        var contentObject = greatGrandChildContent.gameObject;

                        if (contentObject.GetComponent<Button>() == null) return;
                        _collectionObjectUI[i] = contentObject;
                        i++;
                    }
                }
            }
        }
        
        private void InitializeCollection()
        {
            for (var i = 0; i < _collectionObjectUI.Length; i++)
            {
                var collectionId = i + 1;
                var collectionObject = _collectionObjectUI[i];
                
                InitializeElement(collectionId, collectionObject);
            }
            
            closeButtonUI.onClick.AddListener(CloseCollection);
        }
        
        private void InitializeElement(int id, GameObject collection)
        {
            var letterData = letterContainer.GetLetterDataById(id);
            var collectionController = collection.GetComponent<CollectionController>();
            var fillImage = collection.transform.GetChild(0).gameObject;

            collectionController.InitializeData(letterData);
            fillImage.SetActive(GameDatabase.Instance.LoadLetterConditions(id));
        }
        
        // !-- Core Functionality
        private void CloseCollection()
        {
            FindObjectOfType<AudioManager>().PlayAudio(Musics.ButtonSfx);
            OnCollectionCloseEvent();
            mainMenuPanelUI.SetActive(true);

            simpleScrollSnap.Setup();
            collectionPanelUI.SetActive(false);
        }

        // !-- Helpers/Utilities
        public void SetSelectedCollection(int collectionId)
        {
            _selectedCollectionId = collectionId;
        }


        #endregion
        
    }
}