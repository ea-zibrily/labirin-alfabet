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

        [Header("Reference")]
        [SerializeField] private MenuController menuController;
        [SerializeField] private SelectStageManager selectStageManager;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            InitializeContent();
            
            if (!GameDatabase.Instance.IsAvailableUnlock) return;

            InitializeComponent();
            AnimateUnlockStage();
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
            _stageImageUI = _stageContents[1].transform.GetChild(0).GetComponent<Image>();
            _padlockImageUI = _stageContents[1].transform.GetChild(1).GetComponent<Image>();
        }

        // !-- Core Functionality
        private void AnimateUnlockStage()
        {
            StartCoroutine(UnlockStageRoutine());
        }

        private IEnumerator UnlockStageRoutine()
        {
            yield return new WaitForSeconds(unlockDelayDuration);

            blockerPanelUI.SetActive(true);
            menuController.OnPlayButton();
            yield return new WaitForSeconds(eachDelayDuration);

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


        #endregion
    }
}
