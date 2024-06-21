using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KevinCastejon.MoreAttributes;
using Alphabet.Data;
using Alphabet.Stage;
using Alphabet.Gameplay.EventHandler;

namespace Alphabet.Item
{
    public class LetterInterfaceManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("UI")] 
        [SerializeField] private GameObject letterPanel;
        [SerializeField] private GameObject[] letterImageUI;
        [SerializeField] [ReadOnly] private int amountOfLetter;

        private GameObject[] _letterFillImage;
        private int _currentTakenLetter;
        public int ItemIndex { get; private set; }
        public GameObject[] LetterImageUI => letterImageUI;

        // Event
        public event Action<int> OnLetterTaken;
        public event Action<int> OnLetterLost;

        [Header("Reference")] 
        private LetterManager _letterManager;
        private LetterFillAnimation _letterAnimation;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _letterManager = GetComponent<LetterManager>();
            _letterAnimation = GetComponent<LetterFillAnimation>();
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
            amountOfLetter = _letterManager.LetterSpawns[StageManager.Instance.CurrentStageIndex].AmountOfLetter;
            _currentTakenLetter = 0;
            
            for (var i = 0; i < amountOfLetter; i++)
            {
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
            StartCoroutine(TakeLetterRoutine(itemId));
        }

        private IEnumerator TakeLetterRoutine(int itemId)
        {
            var itemIndex = itemId - 1;
            ItemIndex = itemIndex;
            _currentTakenLetter++;
            yield return _letterAnimation.AnimateLetterRoutine(_letterFillImage[itemIndex]);
            
            if (_currentTakenLetter >= amountOfLetter)
            {
                GameEventHandler.ObjectiveClearEvent();
            }
        }
        
        private void LostLetter(int itemId)
        {
            StartCoroutine(LostLetterRoutine(itemId));
        }

        private IEnumerator LostLetterRoutine(int itemId)
        {
            var itemIndex = itemId - 1;
            if (_letterAnimation.IsAnimate && _letterAnimation.TargetFill == _letterFillImage[itemIndex])
            {
                yield return new WaitForSeconds(_letterAnimation.LerpDuration);
            }

            _letterFillImage[itemIndex].SetActive(false);
            _currentTakenLetter--;
        }

        #endregion
    }
}