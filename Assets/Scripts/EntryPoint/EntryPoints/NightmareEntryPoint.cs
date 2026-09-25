using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace EntryPoint.EntryPoints {
    public class NightmareEntryPoint : EntryPoint {
        [SerializeField] private PlayableDirector director;

        private int choice;
        
        public override bool _isSceneLoaded {
            get => isSceneLoaded;
            protected set => isSceneLoaded = value;
        }

        public static bool isSceneLoaded { get; protected set; }
    
        public event Action GoToGameplayScene;

        public override IEnumerator Run() {
            director.Play();

            _isSceneLoaded = true;

            yield return this;
        }

        public void MakeRightChoice() {
            choice = 0;
            PlayerPrefs.SetInt("Choice", choice);
            PlayerPrefs.Save();
        }

        public void MakeLeftChoice() {
            choice = 1;
            PlayerPrefs.SetInt("Choice", choice);
            PlayerPrefs.Save();
        }

        public void GoToGamePlayButtonClick() {
            StartCoroutine(GoToGameplay());
        }

        private IEnumerator GoToGameplay() {
            yield return new WaitForSeconds(5);
            GoToGameplayScene?.Invoke(); 
        }
    }
}
