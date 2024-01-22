using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using LabirinKata.Enum;
using LabirinKata.DesignPattern.Singleton;

namespace LabirinKata.Managers
{
    public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
    {
        #region Variable
    
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
        
        //-- Initialization
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
        
        //-- Core Functionality
        public void LoadSelectedScene(SceneState sceneState)
        {
            // FindObjectOfType<AudioManager>().PlayAudio(AudioList.SFX_Click);
            // Time.timeScale = 1;
            
            switch (sceneState)
            {
                case SceneState.MainMenu:
                    OpenMainMenuScene();
                    break;
                case SceneState.CollectionMenu:
                    OpenCollectionMenuScene();
                    break;
                case SceneState.CurrentLevel:
                    OpenCurrentGameScene();
                    break;
                case SceneState.NextLevel:
                    OpenNextGameScene();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sceneState), sceneState, null);
            }
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
                Invoke ("LoadCurrentGame", 0.5f);
            });
        }
        
        private void OpenNextGameScene()
        {
            sceneFader.gameObject.SetActive (true);

            LeanTween.alpha(sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, 0.5f).setOnComplete (() => {
                Invoke ("LoadNextGame", 0.5f);
            });
        }
        
        private void LoadCurrentGame()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void LoadNextGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        #endregion

    }
}
