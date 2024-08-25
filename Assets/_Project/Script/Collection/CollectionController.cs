using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using KevinCastejon.MoreAttributes;
using Alphabet.Enum;
using Alphabet.Data;
using Alphabet.Letter;
using Alphabet.Database;
using Alphabet.Managers;

namespace Alphabet.Collection
{
    public class CollectionController : MonoBehaviour
    {
        #region Struct

        [Serializable]
        private struct TweenData
        {
            public Vector3 tweenVector;
            public float tweenDuration;
        }

        #endregion

        #region Fields & Properties

        [Header("Data")] 
        [SerializeField] [ReadOnly] private int collectionId;
        [SerializeField] [ReadOnly] private string collectionName;
        
        private LetterData _letterData;
        private bool _canInteract;

        [Header("Tweening")]
        [SerializeField] private TweenData lockTween;
        [SerializeField] private TweenData unlockTween;
        [SerializeField] private float allTweenDuration = 0.5f;
        private Vector3 _defaultScaling;

        [Header("UI")]
        [SerializeField] private Image outerImageUI;
        [SerializeField] private Image fillImageUI;

        private RectTransform _rectTransform;
        private Button _collectionButtonUI;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _collectionButtonUI = GetComponent<Button>();
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            CollectionEventHandler.OnCollectionClose += OnCloseCollection;
        }

        private void OnDisable()
        {
            CollectionEventHandler.OnCollectionClose -= OnCloseCollection;
        }

        private void Start()
        {
            InitializeCollection();
        }
        
        #endregion
        
        #region Methods
        
        // !- Initialize
        public void InitializeData(LetterData data)
        {
            // Data
            _letterData = data;
            
            // Set Component
            collectionId = data.LetterId;
            collectionName = data.LetterName;
            outerImageUI.sprite = data.LetterSprite;
            fillImageUI.sprite = data.LetterSprite;
        }

        private void InitializeCollection()
        {
            if (_letterData == null)
            {
                Debug.LogWarning("letterny null brok");
                return;
            }

            EnableInteract();
            _defaultScaling = _rectTransform.localScale;
            _collectionButtonUI.onClick.AddListener(OnCollectionClicked);
        }
        
        // !- Core
        private void OnCollectionClicked()
        {
            if (!_canInteract) return;

            DisableInteract();
            LetterAudio.StopAudioEvent();
            StartCoroutine(ClickFeedbackRoutine());
        }

        private IEnumerator ClickFeedbackRoutine()
        {
            var hasCollected = GameDatabase.Instance.LoadLetterConditions(collectionId);
            TweenData tweenData = hasCollected ? unlockTween : lockTween;

            // Play button SFX if not collected
            if (!hasCollected) 
                AudioManager.Instance.PlayAudio(Musics.LockedLetterSfx);

            // Scale up with elastic ease
            LeanTween.scale(gameObject, tweenData.tweenVector, allTweenDuration)
                .setEase(LeanTweenType.easeOutElastic)
                .setOnComplete(() =>
                {
                    // Play collection audio if selected and collected
                    if (hasCollected)
                        LetterAudio.PlayAudioEvent(collectionId);
                });

            yield return new WaitForSeconds(tweenData.tweenDuration);
            // Scale down with elastic ease
            LeanTween.scale(gameObject, _defaultScaling, allTweenDuration)
                .setEase(LeanTweenType.easeOutElastic)
                .setOnComplete(() =>
                {
                    // Enable interaction
                    EnableInteract();
                });
        }
        
        private void OnCloseCollection()
        {
            EnableInteract();
            LeanTween.cancel(gameObject);
            _rectTransform.localScale = _defaultScaling;
        }
        
        // !- Helper
        private void EnableInteract()
        {
            _canInteract = true;
            _collectionButtonUI.interactable = true;
        }

        private void DisableInteract()
        {
            _canInteract = false;
            _collectionButtonUI.interactable = false;
        }

        #endregion
        
    }
}