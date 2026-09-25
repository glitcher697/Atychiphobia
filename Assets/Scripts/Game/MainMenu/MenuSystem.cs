using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.MainMenu {
    public class MenuSystem : MonoBehaviour {
        [SerializeField] private Button continueButton;
        
        public event Action GoToGamePlaySceen;

        public void StartGamePlaySceen() {
            PlayerPrefs.SetInt("CurrentDay", 1);
            PlayerPrefs.Save();
            GoToGamePlaySceen?.Invoke();
        }

        public void Continue() {
            GoToGamePlaySceen?.Invoke();
        }

        public void SettingMenu(GameObject gameObject) =>
            gameObject.SetActive(!gameObject.activeSelf);

        public void Exit() =>
            Application.Quit();

        private void Start() {
            if (PlayerPrefs.GetInt("CurrentDay") == 1) return;
            if (PlayerPrefs.GetInt("CurrentDay") == 0) return;

            continueButton.interactable = true;
            continueButton.image.color = Color.white;
        }
    }
}