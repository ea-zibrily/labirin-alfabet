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
using Spine.Unity;

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
            _buttonUI.onClick.AddListener(OnCollectionClicked);
        }
        
        // !- Core
        private void OnCollectionClicked()
        {
            if (!_canInteract) return;

            DisableInteract();
            LetterAudioManager.StopAudioEvent();
            _collectionManager.SetSelectedCollection(collectionId);            
            StartCoroutine(ClickFeedbackRoutine());
        }

        private IEnumerator ClickFeedbackRoutine()
        {
            var hasCollected = GameDatabase.Instance.LoadLetterConditions(collectionId);
            TweenData tweenData = hasCollected ? unlockTween : lockTween;

            // Play button SFX if not collected
            if (!hasCollected) FindObjectOfType<AudioManager>().PlayAudio(Musics.LockedLetterSfx);

            // Scale up with elastic ease
            LeanTween.scale(gameObject, tweenData.tweenVector, allTweenDuration)
                .setEase(LeanTweenType.easeOutElastic)
                .setOnComplete(() =>
                {
                    // Play collection audio if selected and collected
                    if (collectionId == _collectionManager.SelectedCollectionId && hasCollected)
                    {
                        LetterAudioManager.PlayAudioEvent(collectionId);
                    }
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
            _buttonUI.interactable = true;
        }

        private void DisableInteract()
        {
            _canInteract = false;
            _buttonUI.interactable = false;
        }

        #endregion
        
    }
}