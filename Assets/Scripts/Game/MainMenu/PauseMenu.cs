using System.Collections;
using System.Collections.Generic;
using EntryPoint;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.MainMenu {
    public class PauseMenu : MonoBehaviour {

        [SerializeField]
        private GameObject SettingMenu;

       // public bool isPauseMenuActive;

        public void OpenSettingMenu() => SettingMenu.SetActive(!SettingMenu.activeSelf);

        public IEnumerator<PauseMenu> Init() {
            if(SceneManager.GetActiveScene().name == SceneNames.MAIN_MENU || 
               SceneManager.GetActiveScene().name == SceneNames.BOOT) {
                gameObject.SetActive(false);
                yield return null;
            }
            
            yield return this;
        }
        
    }
}