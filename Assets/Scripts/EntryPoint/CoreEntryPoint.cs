using System.Collections;
using EntryPoint.EntryPoints;
using Game.GamePlay.Root;
using Game.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Object = UnityEngine.Object;

namespace EntryPoint {
    public class CoreEntryPoint {
        private static CoreEntryPoint _instance;
        private static Coroutines _coroutines;
        private static UIRootView _uiRootView;
        private static EditorPreferences _editorPreferences;
        private static AudioListener _audioListener;
        private static Settings _settings;
    
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutoStartGame() { 
            _instance = new CoreEntryPoint();
            _instance.StartGame();
        }

        private CoreEntryPoint() {
            _settings = new Settings();
            
            _coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines.gameObject);

            _editorPreferences = new GameObject("EditorPreferences").AddComponent<EditorPreferences>();
            Object.DontDestroyOnLoad(_editorPreferences.gameObject);

            _audioListener = new GameObject("AudioListener").AddComponent<AudioListener>();
            Object.DontDestroyOnLoad(_audioListener.gameObject);

            var prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
            _uiRootView = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(_uiRootView.gameObject);
        }

        private void StartGame() {
            _uiRootView.settingMenu.InitSettings();
            
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == SceneNames.GAMEPLAY) {
                _coroutines.StartCoroutine(LoadAndStartGamePlay());
                return;
            }

            if (sceneName == SceneNames.MAIN_MENU) {
                _coroutines.StartCoroutine(LoadAndStartMainMenu());
                return;
            }

            if (sceneName == SceneNames.NIGHTMARE) {
                _coroutines.StartCoroutine(LoadAndStartNightmareScene());
                return;
            }
            if (sceneName != SceneNames.BOOT) {
                return;
            }
#endif
            
            _coroutines.StartCoroutine(LoadAndStartMainMenu());
        }

        private IEnumerator LoadAndStartGamePlay() {
            _uiRootView.ShowLoadingScreen();

            yield return LoadScene(SceneNames.BOOT);
            yield return LoadScene(SceneNames.GAMEPLAY);
        
            var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
            
            yield return sceneEntryPoint.Init(_uiRootView);
            yield return sceneEntryPoint.Run();

            _uiRootView.Init(sceneEntryPoint);
        
            sceneEntryPoint.GoToNightmareScene += () => {
                _coroutines.StartCoroutine(LoadAndStartNightmareScene());
            };

            sceneEntryPoint.RestartScene += () => {
                _coroutines.StartCoroutine(LoadAndStartGamePlay());
            };

            sceneEntryPoint.GoToMainMenuScene += () => {
                _coroutines.StartCoroutine(LoadAndStartMainMenu());
            };
        
            _uiRootView.HideLoadingScreen();
            
            _uiRootView.settingMenu.InitSettings();
        }

        private IEnumerator LoadAndStartNightmareScene() {
            _uiRootView.ShowLoadingScreen();

            yield return LoadScene(SceneNames.BOOT);
            yield return LoadScene(SceneNames.NIGHTMARE);

            var sceneEntryPoint = Object.FindFirstObjectByType<NightmareEntryPoint>();
            
            yield return sceneEntryPoint.Run();
            
            _uiRootView.Init(sceneEntryPoint);
            
            sceneEntryPoint.GoToGameplayScene += () => {
                _coroutines.StartCoroutine(LoadAndStartGamePlay());
            };
            
            _uiRootView.HideLoadingScreen();
            
            _uiRootView.settingMenu.InitSettings();
        }

        private IEnumerator LoadAndStartMainMenu() {
            _uiRootView.ShowLoadingScreen();

            yield return LoadScene(SceneNames.BOOT);
            yield return LoadScene(SceneNames.MAIN_MENU);

            var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            
            yield return sceneEntryPoint.Run();
            
            _uiRootView.Init(sceneEntryPoint);
            
            sceneEntryPoint.GoToGameplayScene += () => {
                _coroutines.StartCoroutine(LoadAndStartGamePlay());
            };
        
            _uiRootView.HideLoadingScreen();
            
            _uiRootView.settingMenu.InitSettings();
        }

        private IEnumerator LoadScene(string SceneName) {
            yield return SceneManager.LoadSceneAsync(SceneName);
        }
    }
}
