using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KevinCastejon.MoreAttributes;
using LabirinKata.Gameplay.EventHandler;
using LabirinKata.Stage;

namespace LabirinKata.Item.Letter
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

        public int CurrentTakenLetter => _currentTakenLetter;
        
        //-- Event
        public event Action<int> OnLetterTaken;
        public event Action<int> OnLetterLost;

        [Header("Reference")] 
        private LetterManager _letterManager;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            var letterManagerObject = GameObject.FindGameObjectWithTag("LetterManager");
            _letterManager = letterManagerObject.GetComponentInChildren<LetterManager>();
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

            currentAmountOfLetter = _letterManager.LetterSpawns[StageManager.Instance.CurrentStageIndex].AmountOfLetter;
            _currentTakenLetter = 0;
        }
        
        //-- Core Functionality
        public void TakeLetterEvent(int itemId) => OnLetterTaken?.Invoke(itemId);
        public void LostLetterEvent(int itemId) => OnLetterLost?.Invoke(itemId);
        
        private void TakeLetter(int itemId)
        {
            var itemIndex = itemId - 1;
            letterImageUI[itemIndex].SetActive(true);
            _currentTakenLetter++;
            
            if (_currentTakenLetter >= currentAmountOfLetter || IsAllLetterActive())
            {
                GameEventHandler.ObjectiveClearEvent();
            }
        }
        
        private void LostLetter(int itemId)
        {
            var itemIndex = itemId - 1;
            letterImageUI[itemIndex].SetActive(false);
            _currentTakenLetter--;
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