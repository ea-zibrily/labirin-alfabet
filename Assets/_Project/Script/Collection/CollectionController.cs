using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using KevinCastejon.MoreAttributes;
using Alphabet.Enum;
using Alphabet.Data;
using Alphabet.Database;
using Alphabet.Managers;
using Alphabet.Letter;

namespace Alphabet.Collection
{
    public class CollectionController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Data")] 
        [SerializeField] [ReadOnly] private int collectionId;
        [SerializeField] [ReadOnly] private string collectionName;
        
        private LetterData _letterData;
        private bool _canInteract;

        [Header("Tweening")]
        [SerializeField] private Vector3 targetScaling;
        [SerializeField] private float tweeningDuration;
        [SerializeField] private float holdTweenDuration;
        private Vector3 _defaultScaling;

        [Header("UI")]
        [SerializeField] private Image outerImageUI;
        [SerializeField] private Image fillImageUI;
        private RectTransform _rectTransform;

        [Header("Reference")]
        private CollectionManager _collectionManager;
        private Button _buttonUI;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _buttonUI = GetComponent<Button>();
            _rectTransform = GetComponent<RectTransform>();
            _collectionManager = GameObject.FindGameObjectWithTag("Collection").GetComponentInChildren<CollectionManager>();
        }

        private void OnEnable()
        {
            _collectionManager.OnCollectionClose += OnCloseCollection;
        }

        private void OnDisable()
        {
            _collectionManager.OnCollectionClose -= OnCloseCollection;
        }

        private void Start()
        {
            InitializeCollection();
        }
        
        #endregion
        
        #region Methods
        
        // !-- Initialization
        public void InitializeData(LetterData data)
        {
            // Data
            _letterData = data;
            
            // Set Component
            SetId(_letterData.LetterId);
            SetCollectionName(_letterData.LetterName);
            SetSprite(_letterData.LetterSprite);
        }

        private void InitializeCollection()
        {
            if (_letterData == null)
            {
                Debug.LogWarning("letterny null brok");
                return;
            }

            _canInteract = true;
            _defaultScaling = _rectTransform.localScale;
            _buttonUI.interactable = true;
            _buttonUI.onClick.AddListener(OnCollectionClicked);
        }
        
        // !-- Core Functionality
        private void OnCollectionClicked()
        {
            if (!_canInteract) return;

            LetterAudioManager.StopAudioEvent();
            _canInteract = false;
            _buttonUI.interactable = false;
            _collectionManager.SetSelectedCollection(collectionId);            
            StartCoroutine(ClickFeedbackRoutine());
        }

        private IEnumerator ClickFeedbackRoutine()
        {
            var hasCollected = GameDatabase.Instance.LoadLetterConditions(collectionId);
            var tweenDuration = hasCollected ? holdTweenDuration : tweeningDuration;

            // Play button SFX if not collected
            if (!hasCollected)
            {
                FindObjectOfType<AudioManager>().PlayAudio(Musics.LockedLetterSfx);
            }

            // Scale up with elastic ease
            LeanTween.scale(gameObject, targetScaling, tweeningDuration)
                .setEase(LeanTweenType.easeOutElastic)
                .setOnComplete(() =>
                {
                    // Play collection audio if selected and collected
                    if (collectionId == _collectionManager.SelectedCollectionId && hasCollected)
                    {
                        LetterAudioManager.PlayAudioEvent(collectionId);
                    }
                });

            yield return new WaitForSeconds(tweenDuration);
            // Scale down with elastic ease
            LeanTween.scale(gameObject, _defaultScaling, tweeningDuration)
                .setEase(LeanTweenType.easeOutElastic)
                .setOnComplete(() =>
                {
                    // Enable interaction
                    _canInteract = true;
                    _buttonUI.interactable = true;
                });
        }
        
        private void OnCloseCollection()
        {
            _canInteract = true;
            _buttonUI.interactable = true;

            LeanTween.cancel(gameObject);
            _rectTransform.localScale = _defaultScaling;
        }
        
        // !-- Helper/Utilities
        private void SetId(int letterId)
        {
            collectionId = letterId;
        }

        private void SetCollectionName(string letterName)
        {
            collectionName = letterName;
        }

        private void SetSprite(Sprite letterSprite)
        {
            outerImageUI.sprite = letterSprite;
            fillImageUI.sprite = letterSprite;
        }

        #endregion
        
    }
}