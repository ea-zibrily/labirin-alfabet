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
        
        private bool _isTutorialStage;
        private string _currentStage;
        private string _currentStageNumber;
        private string _currentSubStage;
        
        #endregion

        #region MonoBehavior Callbacks
        
        private void Start()
        {
            // Reference
            tagCanvasGroup = notificationCanvasGroup.transform.GetChild(0).GetComponent<CanvasGroup>();
            _isTutorialStage = StageManager.Instance.CurrentStage == StageName.Tutorial;

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
            string markText;
            _currentStageNumber = GetCurrentStage(StageManager.Instance.CurrentStage);
            _currentSubStage = GetCurrentSubStage(StageManager.Instance.CurrentStageNum);

            if (_isTutorialStage)
                markText = _currentStageNumber;
            else
                markText = _currentStageNumber + " - " +_currentSubStage;

            markTextUI.text = markText;
        }
        
        private void SetNotification()
        {
            string stageNumText;
            _currentStage = StageHelper.GetStageNameByEnum(StageManager.Instance.CurrentStage);

            if (_isTutorialStage)
                stageNumText = _currentStageNumber + " Stage";
            else
                stageNumText = _currentStageNumber + " - " +_currentSubStage;

            stageNameTextUI.text = _currentStage.ToUpper();
            stageNumberTextUI.text = stageNumText;
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
        private string GetCurrentStage(StageName level)
        {
            return level switch
            {
                StageName.Tutorial => "Tutorial",
                StageName.Gua_Aksara => "Stage 1",
                StageName.Hutan_Abjad => "Stage 2",
                StageName.Kuil_Litera => "Stage 3",
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
        }

        private string GetCurrentSubStage(StageNum stage)
        {
            return stage switch
            {
                StageNum.Stage_1 => "1",
                StageNum.Stage_2 => "2",
                StageNum.Stage_3 => "3",
                _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, null)
            };
        }

        #endregion
    }
}