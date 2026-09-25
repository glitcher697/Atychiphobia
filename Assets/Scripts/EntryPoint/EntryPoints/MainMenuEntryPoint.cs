using System;
using System.Collections;
using System.Collections.Generic;
using Game.MainMenu;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EntryPoint.EntryPoints {
    public class MainMenuEntryPoint : EntryPoint {
        [SerializeField] private MenuSystem menuSystem;
       // [SerializeField] private SettingMenu settingMenu;
       
       public event Action GoToGameplayScene;
       
       public override bool _isSceneLoaded {
           get => isSceneLoaded;
           protected set => isSceneLoaded = value;
       }

       public static bool isSceneLoaded { get; protected set; }

       public override IEnumerator Run() {
           menuSystem.GoToGamePlaySceen += () => {
               GoToGameplayScene?.Invoke();
           };

           _isSceneLoaded = true;

           Cursor.visible = true;
           Cursor.lockState = CursorLockMode.None;
            
           yield return this;
       }

        
    }
}
