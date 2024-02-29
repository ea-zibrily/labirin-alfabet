using System;
using UnityEngine;
using UnityEngine.UI;
using KevinCastejon.MoreAttributes;
using LabirinKata.Data;

namespace LabirinKata.Collection
{
    public class CollectionController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Data")] 
        [SerializeField] [ReadOnly] private int letterId;
        [SerializeField] [ReadOnly] private string letterName;
        
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

        private void OnEnable() 
        {
            _collectionManager.SimpleScrollSnap.OnSnappingBegin += StopAudio;
        }

        private void OnDisable() 
        {
            _collectionManager.SimpleScrollSnap.OnSnappingBegin -= StopAudio;
        }
        
        private void Start()
        {
            InitializeCollection();
        }
        
        #endregion
        
        #region Labirin Kata Callbacks
        
        // !-- Initialization
        public void InitializeData(LetterData data)
        {
            // Data
            _letterData = data;
            Debug.LogWarning(_letterData);

            // Component
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
            _buttonUI.onClick.AddListener(ClickObject);
        }
        
        // !-- Core Functionality
        private void ClickObject()
        {
            if (!_canInteract) return;

            _canInteract = false;
            LeanTween.scale(gameObject, targetScaling, 0.5f).
                    setEase(LeanTweenType.easeOutElastic).setOnComplete(() =>
                    {
                        _collectionAudioManager.PlayAudio(letterId);
                    });
            
            LeanTween.scale(gameObject, _defaultScaling, 0.5f).setDelay(1.5f).
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
         private void SetId(int letterId)
        {
            this.letterId = letterId;
        }

        private void SetCollectionName(string letterName)
        {
            this.letterName = letterName;
        }

        private void SetSprite(Sprite letterSprite)
        {
            outerImageUI.sprite = letterSprite;
            fillImageUI.sprite = letterSprite;
        }

        #endregion
        
    }
}