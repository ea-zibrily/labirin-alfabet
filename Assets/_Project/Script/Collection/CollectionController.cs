using System;
using UnityEngine;
using UnityEngine.UI;
using KevinCastejon.MoreAttributes;
using Alphabet.Data;
using System.Collections;

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
        private Button _buttonUI;

        [Header("Tweening")]
        [SerializeField] private Vector3 targetScaling;
        [SerializeField] private float tweeningDuration;
        private Vector3 _defaultScaling;
        
        [Header("Reference")]
        [SerializeField] private Image outerImageUI;
        [SerializeField] private Image fillImageUI;
        private CollectionManager _collectionManager;
        private CollectionAudioManager _collectionAudioManager;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _buttonUI = GetComponent<Button>();

            var collectionObject = GameObject.FindGameObjectWithTag("Collection");
            _collectionManager = collectionObject.GetComponentInChildren<CollectionManager>();
            _collectionAudioManager = collectionObject.GetComponentInChildren<CollectionAudioManager>();
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
            _defaultScaling = GetComponent<RectTransform>().localScale;
            _buttonUI.onClick.AddListener(OnCollectionClicked);
        }
        
        // !-- Core Functionality
        private void OnCollectionClicked()
        {
            if (!_canInteract) return;

            _canInteract = false;
            Debug.Log("click broh");
            StopAudio();
            StartCoroutine(ClickFeedbackRoutine());
        }

        private IEnumerator ClickFeedbackRoutine()
        {
            var index = collectionId - 1;
            var scrollSnap = _collectionManager.SimpleScrollSnap;
            if (scrollSnap.CenteredPanel != index)
            {
                scrollSnap.GoToPanel(index);
                yield return new WaitForSeconds(0.15f);
            }

            LeanTween.scale(gameObject, targetScaling, 0.5f).
                    setEase(LeanTweenType.easeOutElastic).setOnComplete(() =>
                    {
                        _collectionAudioManager.PlayAudio(collectionId);
                    });
            
            LeanTween.scale(gameObject, _defaultScaling, 0.5f).setDelay(tweeningDuration).
                    setEase(LeanTweenType.easeOutElastic).setOnComplete(() =>
                    {
                        _canInteract = true;
                    });       
        }

        private void StopAudio()
        {
            if (!_collectionAudioManager.IsAudioPlaying()) return;
            _collectionAudioManager.StopAudio();
        }

        // !-- Helper/Utilities
        private void SetId(int letterId) => collectionId = letterId;

        private void SetCollectionName(string letterName) => collectionName = letterName;

        private void SetSprite(Sprite letterSprite)
        {
            outerImageUI.sprite = letterSprite;
            fillImageUI.sprite = letterSprite;
        }

        #endregion
        
    }
}