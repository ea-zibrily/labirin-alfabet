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
            StartFader();
        }
        
        #endregion

        #region Scene Loader Callbacks
        
        public void LoadCurrentScene()
        {
            FindObjectOfType<AudioManager>().PlayAudio(AudioList.SFX_Click);
            OpenCurrentScene();
        }
        
        public void LoadNextScene()
        {
            FindObjectOfType<AudioManager>().PlayAudio(AudioList.SFX_Click);
            OpenNextScene();
        }
        
        private void StartFader()
        {
            sceneFader.gameObject.SetActive (true);
        
            LeanTween.alpha (sceneFader, 1, 0);
            LeanTween.alpha (sceneFader, 0, 1f).setOnComplete (() => {
                sceneFader.gameObject.SetActive (false);
            });
        }

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

        public void OpenNextStage(IEnumerator routine)
        {
            Time.timeScale = 1;
            sceneFader.gameObject.SetActive (true);

            LeanTween.alpha(sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, 0.5f).setOnComplete (() =>
            {
                StartCoroutine(routine);
            });
        }
        
        private void OpenCurrentScene()
        {
            Time.timeScale = 1;
            sceneFader.gameObject.SetActive (true);

            LeanTween.alpha(sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, 0.5f).setOnComplete (() => {
                Invoke ("LoadCurrentGame", 0.5f);
            });
        }
        
        private void OpenNextScene()
        {
            Time.timeScale = 1;
            sceneFader.gameObject.SetActive (true);

            LeanTween.alpha(sceneFader, 0, 0);
            LeanTween.alpha (sceneFader, 1, 0.5f).setOnComplete (() => {
                Invoke ("LoadNextGame", 0.5f);
            });
        }
        
        private void LoadCurrentGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        private void LoadNextGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        #endregion

    }
}
