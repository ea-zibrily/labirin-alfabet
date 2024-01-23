using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KevinCastejon.MoreAttributes;
using LabirinKata.Enum;
using LabirinKata.Stage;
using LabirinKata.Gameplay.EventHandler;

namespace LabirinKata.Entities.Item
{
    public class LetterUIManager : MonoBehaviour
    {
        #region Struct

        [Serializable]
        private struct LetterImages
        {
            public StageList StageName;
            public int AmountOfLetter;
            public Sprite[] LetterSprites;
        }
        
        #endregion
        
        #region Variable

        [Header("Letter UI")] 
        [SerializeField] private LetterImages[] letterSprites;
        [SerializeField] private GameObject[] letterImageUI;
        [SerializeField] [ReadOnly] private int currentAmountOfLetter;
        
        private GameObject[] _letterBorderImage;
        private int _currentTakenLetter;
        
        //-- Event
        public event Action<int> OnLetterTaken;
        public event Action OnInitializeLetterUI;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            OnLetterTaken += UpdateTakenLetter;
            OnInitializeLetterUI += InitializeLetterUI;
        }

        private void OnDisable()
        {
            OnLetterTaken -= UpdateTakenLetter;
            OnInitializeLetterUI -= InitializeLetterUI;
        }

        private void Start()
        {
            InitializeLetterUI();
        }
        
        #endregion

        #region Event Callbacks

        //-- Core Functionality
        public void LetterTakenEvent(int itemId) => OnLetterTaken?.Invoke(itemId);
        public void InitializeLetterEvent() => OnInitializeLetterUI?.Invoke();

        #endregion
        
        #region Labirin Kata Callbacks
        
        //-- Initialization
        private void InitializeLetterUI()
        {
            var currentStage = StageManager.Instance.CurrentStageIndex;
            _letterBorderImage ??= new GameObject[letterImageUI.Length];

            for (var i = 0; i < letterImageUI.Length; i++)
            {
                var letterBorder = letterImageUI[i].transform.parent.gameObject;
                var letterSprite = letterSprites[currentStage].LetterSprites[i];
                
                _letterBorderImage[i] = letterBorder;
                _letterBorderImage[i].GetComponent<Image>().sprite = letterSprite;
                
                letterImageUI[i].GetComponent<Image>().sprite = letterSprite;
                letterImageUI[i].SetActive(false);
            }
            
            currentAmountOfLetter = letterSprites[currentStage].AmountOfLetter;
            _currentTakenLetter = 0;
        }
        
        //-- Core Functionality
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