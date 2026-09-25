using System;
using System.Collections;
using EntryPoint.EntryPoints;
using Game.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace EntryPoint {
    public class UIRootView : MonoBehaviour {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private Animator transitionScreen;
        [SerializeField] private GameObject pauseMenu;
        
        public event Action GoToMainMenu;
        public SettingMenu settingMenu;
        
        private bool isPauseMenuActive;

        private EntryPoints.EntryPoint _sceneEntryPoint;

        public void Init(EntryPoints.EntryPoint entryPoint) {
            _sceneEntryPoint = entryPoint;
        }
        
        private void Start() {
            HideLoadingScreen();
            HideTransition();
            
            if(SceneManager.GetActiveScene().name != SceneNames.GAMEPLAY) return;
            
            HidePauseMenu();

            GoToMainMenu += () => {
                HidePauseMenu();
                GameManager.Instance?.GetChangePlayerActions(true);
            };
        }

        public void ShowLoadingScreen() {
            loadingScreen.SetActive(true);
        }

        public void HideLoadingScreen() {
            loadingScreen.SetActive(false);

            StartCoroutine(ShowTransition());
        }
        
        public IEnumerator ShowTransition() {
            transitionScreen.gameObject.SetActive(true);
            transitionScreen.Play("Blendin");

            float duration = transitionScreen.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(duration);
            
            HideTransition();
        }

        public void HideTransition() {
            transitionScreen.gameObject.SetActive(false);
        }

        public void ShowPauseMenu() {
            if(!_sceneEntryPoint) 
                return;
            
            GameManager.Instance?.GetChangePlayerActions(false);
            GameManager.Instance?.PauseMusic();
            GameManager.Instance?.HideUI();
            pauseMenu.gameObject.SetActive(true);
        }

        public void HidePauseMenu() {
            if(!_sceneEntryPoint) 
                return;
            
            GameManager.Instance?.GetChangePlayerActions(true);
            GameManager.Instance?.UnPauseMusic();
            GameManager.Instance?.ShowUI();
            pauseMenu.gameObject.SetActive(false);
        }

        public void Exit() {
            HidePauseMenu();
            GoToMainMenu?.Invoke();
        }

        private void Update() {
            if(!_sceneEntryPoint) 
                return;

            if (!_sceneEntryPoint._isSceneLoaded)
                return;
            
            if(_sceneEntryPoint is not GameplayEntryPoint) 
                return;
            
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if(!isPauseMenuActive) ShowPauseMenu();
                
                if(isPauseMenuActive) HidePauseMenu();
                
                isPauseMenuActive = !isPauseMenuActive;
            }
        }
    }
}