﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KevinCastejon.MoreAttributes;
using LabirinKata.Gameplay.EventHandler;
using LabirinKata.Stage;
using LabirinKata.Data;

namespace LabirinKata.Item
{
    public class LetterInterfaceManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Letter UI")] 
        [SerializeField] private GameObject[] letterImageUI;
        [SerializeField] [ReadOnly] private int currentAmountOfLetter;
        
        private GameObject[] _letterFillImage;
        private int _currentTakenLetter;
                
        //-- Event
        public event Action<int> OnLetterTaken;
        public event Action<int> OnLetterLost;

        [Header("Reference")] 
        private LetterManager _letterManager;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _letterManager = GetComponent<LetterManager>();
        }

        private void OnEnable()
        {
            OnLetterTaken += TakeLetter;
            OnLetterLost += LostLetter;
        }
        
        private void OnDisable()
        {
            OnLetterTaken -= TakeLetter;
            OnLetterLost -= LostLetter;
        }

        private void Start()
        {
            InitializeLetterImage();
        }

        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        private void InitializeLetterImage()
        {
            foreach (var image in letterImageUI)
            {
                image.SetActive(false);
            }
        }
        
        // !-- Core Functionality
        public void SetLetterInterface(IReadOnlyList<LetterData> datas)
        {
            _letterFillImage ??= new GameObject[letterImageUI.Length];
            currentAmountOfLetter = _letterManager.LetterSpawns[StageManager.Instance.CurrentStageIndex].AmountOfLetter;
            _currentTakenLetter = 0;
            
            for (var i = 0; i < currentAmountOfLetter; i++)
            {
                Debug.Log($"call interface {datas[i]}");
                var letterSprite = datas[i].LetterSprite;
                var letterFill = letterImageUI[i].transform.GetChild(0).gameObject;
                
                letterImageUI[i].SetActive(true);
                letterImageUI[i].GetComponent<Image>().sprite = letterSprite;
                
                _letterFillImage[i] = letterFill;
                _letterFillImage[i].GetComponent<Image>().sprite = letterSprite;
                _letterFillImage[i].SetActive(false);
            }
        }
        
        public void TakeLetterEvent(int itemId) => OnLetterTaken?.Invoke(itemId);
        public void LostLetterEvent(int itemId) => OnLetterLost?.Invoke(itemId);
        
        private void TakeLetter(int itemId)
        {
            var itemIndex = itemId - 1;
            _letterFillImage[itemIndex].SetActive(true);
            _currentTakenLetter++;
            
            if (_currentTakenLetter >= currentAmountOfLetter)
            {
                GameEventHandler.ObjectiveClearEvent();
            }
        }
        
        private void LostLetter(int itemId)
        {
            var itemIndex = itemId - 1;
            _letterFillImage[itemIndex].SetActive(false);
            _currentTakenLetter--;
        }
        
        #endregion
    }
}