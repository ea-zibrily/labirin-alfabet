using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Alphabet.Enum;
using Alphabet.Gameplay.EventHandler;

namespace Alphabet.Stage
{
    public class StageMarker : MonoBehaviour
    {
        #region Variable

        [Header("Notification")]
        [Range(0f, 2.5f)][SerializeField] private float stayDuration;
        [Range(0f, 2.5f)][SerializeField] private float fadeDuration;

        [Space]
        [SerializeField] private TextMeshProUGUI stageNameTextUI;
        [SerializeField] private TextMeshProUGUI stageNumberTextUI;
        [SerializeField] private CanvasGroup notificationCanvasGroup;

        private CanvasGroup tagCanvasGroup;

        [Header("Marker")] 
        [SerializeField] private TextMeshProUGUI markTextUI;
        
        private string _currentLevel;
        private string _currentStage;
        
        #endregion

        #region MonoBehavior Callbacks
        
        private void Start()
        {
            // Reference
            tagCanvasGroup = notificationCanvasGroup.transform.GetChild(0).GetComponent<CanvasGroup>();

            InitializeNotification();
            TopMarker();
        }
        
        #endregion

        #region Methods
        
        // !-- Initialization
        private void InitializeNotification()
        {
            notificationCanvasGroup.gameObject.SetActive(false);
            notificationCanvasGroup.alpha = 1f;
            tagCanvasGroup.alpha = 0f;
        }

        // !-- Core Functionality
        
        public void ShowNotification() => StartCoroutine(ShowNotificationRoutine());

        public void TopMarker()
        {
            _currentStage = GetCurrentStage(StageManager.Instance.CurrentStageList);
            markTextUI.text = _currentStage;
        }

        private void SetNotification()
        {
            var levelIndex = StageHelper.GetStageIntValue(StageManager.Instance.CurrentLevelList);
            _currentLevel = StageHelper.GetStageStringValue(levelIndex);
            _currentStage = GetCurrentStage(StageManager.Instance.CurrentStageList);

            stageNameTextUI.text = _currentLevel.ToUpper();
            stageNumberTextUI.text = _currentStage;
        }
        
        private IEnumerator ShowNotificationRoutine()
        {
            SetNotification();

            notificationCanvasGroup.gameObject.SetActive(true);            
            LeanTween.alphaCanvas(tagCanvasGroup, 1f, fadeDuration).setEase(LeanTweenType.easeInSine);
            yield return new WaitForSeconds(fadeDuration);

            tagCanvasGroup.alpha = 1f;
            yield return new WaitForSeconds(stayDuration);

            LeanTween.alphaCanvas(notificationCanvasGroup, 0f, fadeDuration).setEase(LeanTweenType.easeInSine)
                    .setOnComplete(() => 
                    {
                        InitializeNotification();
                        GameEventHandler.GameStartEvent();
                    });
        }
        
        // !-- Helpers/Utilities
        private string GetCurrentStage(StageNum stage)
        {
            return stage switch
            {
                StageNum.Stage_1 => "Stage 1",
                StageNum.Stage_2 => "Stage 2",
                StageNum.Stage_3 => "Stage 3",
                _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, null)
            };
        }

        #endregion
    }
}