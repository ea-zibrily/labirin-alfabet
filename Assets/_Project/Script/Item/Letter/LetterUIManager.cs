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
        #region Fields & Properties

        [Header("Letter UI")] 
        [SerializeField] private GameObject[] letterImageUI;
        [SerializeField] [ReadOnly] private int currentAmountOfLetter;
        
        private GameObject[] _letterFillImage;
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

        private void Start()
        {
            InitializeLetterImage();
        }

        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        public void InitializeLetterInterface(IReadOnlyList<GameObject> objects)
        {
            _letterFillImage ??= new GameObject[letterImageUI.Length];
            currentAmountOfLetter = _letterManager.LetterSpawns[StageManager.Instance.CurrentStageIndex].AmountOfLetter;
            _currentTakenLetter = 0;
            
            for (var i = 0; i < currentAmountOfLetter; i++)
            {
                var letterObject = objects[i].GetComponent<SpriteRenderer>();
                var letterFill = letterImageUI[i].transform.GetChild(0).gameObject;
                
                letterImageUI[i].SetActive(true);
                letterImageUI[i].GetComponent<Image>().sprite = letterObject.sprite;
                
                _letterFillImage[i] = letterFill;
                _letterFillImage[i].GetComponent<Image>().sprite = letterObject.sprite;
                _letterFillImage[i].SetActive(false);
            }
        }
        
        private void InitializeLetterImage()
        {
            foreach (var image in letterImageUI)
            {
                image.SetActive(false);
            }
        }
        
        // !-- Core Functionality
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