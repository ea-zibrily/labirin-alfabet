using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Alphabet.Enum;
using Alphabet.Gameplay.Controller;
using Alphabet.DesignPattern.Singleton;

namespace Alphabet.Managers
{
    public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
    {
        #region Fields & Properties
    
        [Header("Interface")]
        [Range(0f, 2f )][SerializeField] private float fadeDuration;
        [SerializeField] private RectTransform sceneFader;

        public float FadeDuration => fadeDuration;

        #endregion
    
        #region MonoBehaviour Callbacks

        private void Start()
        {
            FadeIn();
        }
        
        #endregion

        #region Methods
        
        // !- Initialize
        public void FadeIn()
        {
            sceneFader.gameObject.SetActive (true);
        
            LeanTween.alpha (sceneFader, 1, 0);
            LeanTween.alpha (sceneFader, 0, fadeDuration).setOnComplete (() => {
                sceneFader.gameObject.SetActive (false);
            });
        }
        
        public void FadeOut()
        {
            sceneFader.gameObject.SetActive (true);

            LeanTween.alpha(sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, fadeDuration);
        }
        
        // !- Core
        public void LoadSelectedScene(SceneState sceneState)
        {
            Time.timeScale = 1;
            switch (sceneState)
            {
                case SceneState.MainMenu:
                    OpenMainMenuScene();
                    break;
                case SceneState.CurrentLevel:
                    OpenCurrentGameScene();
                    break;
                case SceneState.NextLevel:
                    OpenNextLevelScene();
                    break;
            }
        }
        
        public void LoadSelectedLevel(int levelIndex)
        {
            Time.timeScale = 1;
            sceneFader.gameObject.SetActive(true);

            AudioController.FadeAudioEvent(isFadeIn: false, FadeDuration);
            LeanTween.alpha(sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, fadeDuration).setOnComplete (() => {
                SceneManager.LoadScene(levelIndex);
            });
        }

        private void OpenMainMenuScene () 
        {
            sceneFader.gameObject.SetActive (true);

            AudioController.FadeAudioEvent(isFadeIn: false, FadeDuration);
            LeanTween.alpha (sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, fadeDuration).setOnComplete (() => {
                SceneManager.LoadScene(0);
            });
        }
        
        private void OpenCurrentGameScene()
        {
            sceneFader.gameObject.SetActive (true);

            LeanTween.alpha(sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, fadeDuration).setOnComplete (() => {
                Invoke (nameof(LoadCurrentGame), fadeDuration);
            });
        }
        
        private void OpenNextLevelScene()
        {
            sceneFader.gameObject.SetActive (true);

            AudioController.FadeAudioEvent(isFadeIn: false, FadeDuration);
            LeanTween.alpha(sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, fadeDuration).setOnComplete (() => {
                Invoke (nameof(LoadNextGame), fadeDuration);
            });
        }
        
        private void LoadCurrentGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        private void LoadNextGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        #endregion

    }
}
