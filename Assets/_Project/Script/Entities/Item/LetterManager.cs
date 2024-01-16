using System;
using KevinCastejon.MoreAttributes;
using LabirinKata.Gameplay.EventHandler;
using UnityEngine;

namespace LabirinKata.Entities.Item
{
    public class LetterManager : MonoBehaviour
    {
        #region Variable
        
        [Header("Objective")] 
        [SerializeField] [ReadOnlyOnPlay] private int amountOfLetter;
        [SerializeField] private GameObject[] letterItemUI;
        
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
        }
        
        #endregion
        
        #region CariHuruf Callbacks
        
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
            
            if (_currentTakenLetter >= amountOfLetter || IsAllLetterActive())
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
            
            return activeLetterNum >= amountOfLetter;
        }
        
        #endregion
    }
}