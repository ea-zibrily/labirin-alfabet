using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KevinCastejon.MoreAttributes;
using LabirinKata.Gameplay.EventHandler;

namespace LabirinKata.Entities.Item
{
    public class LetterUIManager : MonoBehaviour
    {
        #region Variable

        [Header("Letter UI")] 
        [SerializeField] private Sprite[] letterSprites;
        [SerializeField] private GameObject[] letterImageUI;
        [SerializeField] [ReadOnly] private int currentAmountOfLetter;
        
        private GameObject[] _letterBorderImage;
        private int _currentTakenLetter;
        
        //-- Event
        public event Action<int> OnLetterTaken;
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void OnEnable()
        {
            OnLetterTaken += UpdateTakenLetter;
        }
        
        private void OnDisable()
        {
            OnLetterTaken -= UpdateTakenLetter;
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        //-- Initialization
        public void InitializeLetterUI(IReadOnlyList<int> index)
        {
            _letterBorderImage ??= new GameObject[letterImageUI.Length];
            
            for (var i = 0; i < letterImageUI.Length; i++)
            {
                var letterBorder = letterImageUI[i].transform.parent.gameObject;
                var spriteIndex = index[i];
                
                _letterBorderImage[i] = letterBorder;
                _letterBorderImage[i].GetComponent<Image>().sprite = letterSprites[spriteIndex];
                
                letterImageUI[i].GetComponent<Image>().sprite = letterSprites[spriteIndex];
                letterImageUI[i].SetActive(false);
            }
            
            _currentTakenLetter = 0;
        }
        
        //-- Core Functionality
        public void TakeLetterEvent(int itemId) => OnLetterTaken?.Invoke(itemId);

        private void UpdateTakenLetter(int itemId)
        {
            var itemIndex = itemId - 1;
            letterImageUI[itemIndex].SetActive(true);
            _currentTakenLetter++;
            
            if (_currentTakenLetter >= currentAmountOfLetter || IsAllLetterActive())
            {
                GameEventHandler.ObjectiveClearEvent();
            }
        }
        
        //-- Helper/Utilities
        private bool IsAllLetterActive()
        {
            var activeLetterNum = 0;
            foreach (var itemUI in letterImageUI)
            {
                if (itemUI.activeSelf)
                {
                    activeLetterNum++;
                }
            }
            
            return activeLetterNum >= currentAmountOfLetter;
        }
        
        #endregion
    }
}