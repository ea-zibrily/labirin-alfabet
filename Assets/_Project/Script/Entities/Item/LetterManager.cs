using System;
using UnityEngine;
using KevinCastejon.MoreAttributes;
using LabirinKata.Enum;
using LabirinKata.Gameplay.EventHandler;

namespace LabirinKata.Entities.Item
{
    public class LetterManager : MonoBehaviour
    {
        #region Struct

        [Serializable]
        private struct LetterUI
        {
            public StageList StageName;
            public int AmountOfLetter;
            public Sprite[] LetterSprites;
        }

        #endregion
        
        #region Variable

        [Header("Objective")] 
        [SerializeField] private LetterUI[] letterInterface;
        [SerializeField] private GameObject[] letterItemUI;
        [SerializeField] [ReadOnly] private int _currentLetterCount;
        
        public event Action<int> OnLetterTaken;
        private int _currentTakenLetter;
        
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

        private void Start()
        {
            InitializeLetter();
            _currentLetterCount = letterItemUI.Length;
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        public void LetterTakenEvent(int itemId) => OnLetterTaken?.Invoke(itemId);
        
        private void InitializeLetter()
        {
            foreach (var letter in letterItemUI)
            {
                letter.SetActive(false);
            }
            
            _currentTakenLetter = 0;
        }
        
        private void UpdateTakenLetter(int itemId)
        {
            var itemIndex = itemId - 1;
            letterItemUI[itemIndex].SetActive(true);
            _currentTakenLetter++;
            
            if (_currentTakenLetter >= _currentLetterCount || IsAllLetterActive())
            {
                GameEventHandler.ObjectiveClearEvent();
            }
        }
        
        private bool IsAllLetterActive()
        {
            var activeLetterNum = 0;
            foreach (var itemUI in letterItemUI)
            {
                if (itemUI.activeSelf)
                {
                    activeLetterNum++;
                }
            }
            
            return activeLetterNum >= _currentLetterCount;
        }
        
        #endregion
    }
}