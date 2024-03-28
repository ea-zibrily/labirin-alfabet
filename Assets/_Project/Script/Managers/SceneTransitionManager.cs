using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Alphabet.Enum;
using Alphabet.DesignPattern.Singleton;

namespace Alphabet.Managers
{
    public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
    {
        #region Fields & Properties
    
        [Header("Interface")]
        [Range(0f, 2f )][SerializeField] private float fadeDuration;
        [SerializeField] private RectTransform sceneFader;
        
        #endregion
    
        #region MonoBehaviour Callbacks

        private void Start()
        {
            FadeIn();
        }
        
        #endregion

        #region Scene Loader Callbacks
        
        // !-- Initialization
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
        
        // !-- Core Functionality
        public void LoadSelectedScene(SceneState sceneState)
        {
            // FindObjectOfType<AudioManager>().PlayAudio(AudioList.SFX_Click);
            Time.timeScale = 1;
            
            switch (sceneState)
            {
                case SceneState.MainMenu:
                    OpenMainMenuScene();
                    break;
                case SceneState.LevelSelectionMenu:
                    OpenCollectionMenuScene();
                    break;
                case SceneState.CurrentLevel:
                    OpenCurrentGameScene();
                    break;
                case SceneState.NextLevel:
                    OpenNextLevelScene();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sceneState), sceneState, null);
            }
        }
        
        public void LoadSelectedLevel(int levelIndex)
        {
            // FindObjectOfType<AudioManager>().PlayAudio(AudioList.SFX_Click);
            Time.timeScale = 1;
            sceneFader.gameObject.SetActive(true);

            LeanTween.alpha(sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, fadeDuration).setOnComplete (() => {
                SceneManager.LoadScene(levelIndex);
            });
        }

        private void OpenMainMenuScene () 
        {
            sceneFader.gameObject.SetActive (true);
        
            LeanTween.alpha (sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, fadeDuration).setOnComplete (() => {
                SceneManager.LoadScene(0);
            });
        }
        
        private void OpenCollectionMenuScene () 
        {
            sceneFader.gameObject.SetActive (true);
        
            LeanTween.alpha (sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, fadeDuration).setOnComplete (() => {
                SceneManager.LoadScene(1);
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
