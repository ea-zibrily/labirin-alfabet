using System.Collections;
using System.Collections.Generic;
using Alphabet.Database;
using UnityEngine;
using UnityEngine.UI;

namespace Alphabet.UI
{
    public class UnlockController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("UI")]
        [SerializeField] private GameObject blockerPanelUI;
        [SerializeField] private RectTransform stageContentTransformUI;
        [SerializeField] private Button exploreButtonUI;

        private int _panelIndex;
        private RectTransform[] _stageContents;
        private Image _stageImageUI;
        private Image _padlockImageUI;

        [Header("Duration")]
        [SerializeField] private float unlockDelayDuration;
        [SerializeField] private float eachDelayDuration;
        [SerializeField] private float changeColorDuration;

        [Header("Tweening")]
        [SerializeField] private LeanTweenType leanTweenType;
        [SerializeField] private float tweeningDuration;
        [SerializeField] private Vector3 scaleTarget;
        private Vector3 _scaleOrigin;

        [Header("Reference")]
        [SerializeField] private MenuController menuController;
        [SerializeField] private SelectStageManager selectStageManager;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            // if (!GameDatabase.Instance.IsAnimateUnlock) return;
            
            // InitializeContent();
            // InitializeComponent();
            // AnimateUnlockStage();
        }

        #endregion

        #region Methods
        
        // !-- Initialization
        private void InitializeContent()
        {
            var contentNum = stageContentTransformUI.childCount;
            _stageContents ??= new RectTransform[contentNum];

            for (var i = 0; i < contentNum; i++)
            {
                _stageContents[i] = stageContentTransformUI.GetChild(i).GetComponent<RectTransform>();
            }
        }

        private void InitializeComponent()
        {
            _panelIndex = GameDatabase.Instance.LoadLevelClearIndex();
            _stageImageUI = _stageContents[_panelIndex].transform.GetChild(0).GetComponent<Image>();
            _padlockImageUI = _stageContents[_panelIndex].transform.GetChild(1).GetComponent<Image>();
            _scaleOrigin = menuController.PlayButtonUI.transform.localScale;
        }

        // !-- Core Functionality
        private void AnimateUnlockStage()
        {
            StartCoroutine(UnlockStageRoutine());
        }

        private IEnumerator UnlockStageRoutine()
        {
            blockerPanelUI.SetActive(true);
            yield return new WaitForSeconds(unlockDelayDuration);

            var stagePanelDelay = eachDelayDuration * 2;
            var playButtonObj = menuController.PlayButtonUI.gameObject;
            OpenStagePanel(playButtonObj);
            yield return new WaitForSeconds(stagePanelDelay);

            selectStageManager.SimpleScrollSnap.GoToPanel(_panelIndex);
            yield return new WaitForSeconds(eachDelayDuration);

            // TODO: padlock ganti jadi animasi misal udh ada
            _padlockImageUI.gameObject.SetActive(false);
            yield return new WaitForSeconds(eachDelayDuration);

            _stageImageUI.material = null;
            yield return ChangeImageColor(_stageImageUI, changeColorDuration);
            
            blockerPanelUI.SetActive(false);
            ActivateExploreButton();
            GameDatabase.Instance.ResetLevelClearIndex();
        }

        private void ActivateExploreButton()
        {
            exploreButtonUI.interactable = true;
            exploreButtonUI.GetComponent<Image>().material = null;
            exploreButtonUI.GetComponent<CanvasGroup>().enabled = false;
        }


        // !-- Helper/Utilities
        private IEnumerator ChangeImageColor(Image image, float duration)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
                var lerpRatio = elapsedTime / duration;
                image.color = Color.Lerp(image.color, Color.white, lerpRatio);
                if (image.color == Color.white) break;
            }

            image.color = Color.white;
        }

        private void OpenStagePanel(GameObject buttonObject)
        {
            LeanTween.scale(buttonObject, scaleTarget, tweeningDuration).setEase(leanTweenType);
            LeanTween.scale(buttonObject, _scaleOrigin, tweeningDuration)
                .setDelay(tweeningDuration).setEase(leanTweenType).setOnComplete(()=> 
                {
                    menuController.OnPlayButton();
                });
        }


        #endregion
    }
}
