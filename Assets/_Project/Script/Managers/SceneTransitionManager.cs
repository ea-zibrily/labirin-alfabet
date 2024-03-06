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
            LeanTween.alpha (sceneFader, 0, 1f).setOnComplete (() => {
                sceneFader.gameObject.SetActive (false);
            });
        }
        
        public void FadeOut()
        {
            sceneFader.gameObject.SetActive (true);

            LeanTween.alpha(sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, 1f);
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
            LeanTween.alpha (sceneFader, 1, 0.5f).setOnComplete (() => {
                SceneManager.LoadScene(levelIndex);
            });
        }

        private void OpenMainMenuScene () 
        {
            sceneFader.gameObject.SetActive (true);
        
            LeanTween.alpha (sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, 1f).setOnComplete (() => {
                SceneManager.LoadScene(0);
            });
        }
        
        private void OpenCollectionMenuScene () 
        {
            sceneFader.gameObject.SetActive (true);
        
            LeanTween.alpha (sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, 1f).setOnComplete (() => {
                SceneManager.LoadScene(1);
            });
        }
        
        private void OpenCurrentGameScene()
        {
            sceneFader.gameObject.SetActive (true);

            LeanTween.alpha(sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, 0.5f).setOnComplete (() => {
                Invoke (nameof(LoadCurrentGame), 0.5f);
            });
        }
        
        private void OpenNextLevelScene()
        {
            sceneFader.gameObject.SetActive (true);

            LeanTween.alpha(sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, 0.5f).setOnComplete (() => {
                Invoke (nameof(LoadNextGame), 0.5f);
            });
        }
        
        private void LoadCurrentGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        private void LoadNextGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        #endregion

    }
}
