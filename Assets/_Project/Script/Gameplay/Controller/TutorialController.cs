using System.Collections;
using System.Collections.Generic;
using Alphabet.Data;
using Alphabet.Item;
using UnityEngine;
using UnityEngine.UI;

namespace Alphabet.Gameplay.Controller
{
    public class TutorialController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("UI")]
        [SerializeField] private GameObject tutorialPanelUI;
        [SerializeField] private GameObject[] letterObjectivesUI;
        [SerializeField] private Button closeButtonUI;

        private List<LetterData> _letterDatas;

        [Header("References")]
        [SerializeField] private LetterPooler letterPooler;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            closeButtonUI.onClick.AddListener(OnCloseTutorial);
        }

        #endregion

        #region Methods

        // !-- Initialization
        private void InitializeTutorial()
        {
            // Letter datas
            _letterDatas ??= new List<LetterData>();
            _letterDatas.Clear();

            var spawnedDatas = letterPooler.SpawnedLetterDatas;
            _letterDatas.AddRange(spawnedDatas);
            
            // Other
            tutorialPanelUI.SetActive(true);
        }

        // !-- Core Functionality
        public void CallTutorial()
        {
            InitializeTutorial();
            MissionHandler();
        }

        private void MissionHandler()
        {
            var j = 0;
            foreach (var letter in letterObjectivesUI)
            {
                if (j >= _letterDatas.Count - 1) break;

                var letterImage = letter.GetComponent<Image>();
                letterImage.sprite = _letterDatas[j].LetterSprite;
                letter.SetActive(true);
                j++;
            }
        }

        private void OnCloseTutorial()
        {
            tutorialPanelUI.SetActive(false);
            foreach (var letter in letterObjectivesUI)
            {
                if (!letter.activeSelf) continue;
                letter.SetActive(false);
            }
        }

        #endregion
    }
}
