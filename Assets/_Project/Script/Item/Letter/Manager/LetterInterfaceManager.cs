using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KevinCastejon.MoreAttributes;
using Alphabet.Data;
using Alphabet.Stage;
using Alphabet.Gameplay.EventHandler;

namespace Alphabet.Letter
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

        private void Start()
        {
            InitializeLetterImage();
        }

        #endregion
        
        #region Methods
        
        // !- Initialize
        private void InitializeLetterImage()
        {
            foreach (var image in letterImageUI)
            {
                image.SetActive(false);
            }
        }
        
        // !- Core 
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
        
        
        public void TakeLetter(int itemId) => StartCoroutine(TakeLetterRoutine(itemId));

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
        
        public void LostLetter(int itemId) => StartCoroutine(LostLetterRoutine(itemId));

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