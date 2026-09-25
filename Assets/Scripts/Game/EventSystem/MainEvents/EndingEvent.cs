using System;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Playables;

public class EndingEvent : MonoBehaviour {
    [SerializeField] private PlayableDirector goodEndDirector, badEndDirector;
    [SerializeField] private Animator transition;
    [SerializeField] private AudioSource music;

    public Action GoToMenu;
    
    private GameManager _gameManager;

    public IEnumerator Init(GameManager gameManager) {
        _gameManager = gameManager; 
        yield break;
    }

    public IEnumerator StartEvent() {
        transition.enabled = true;
        transition.Play("fading");
        music.Play();
        music.loop = true;
        //_gameManager.HideUI();

        yield return new WaitForSeconds(0.4f);
        
        goodEndDirector.Play();

        yield return new WaitForSeconds(20);
        
        PlayerPrefs.SetInt("CurrentDay", 0);
        PlayerPrefs.Save();
        
        GoToMenu?.Invoke();
    }

    public IEnumerator StartBadEnding() {
        transition.enabled = true;
        transition.Play("fading");
        music.Play();
        music.loop = true;
        //_gameManager.HideUI();

        yield return new WaitForSeconds(0.4f);
        
        badEndDirector.Play();
        yield return new WaitForSeconds(11);
        
        PlayerPrefs.SetInt("CurrentDay", 0);
        PlayerPrefs.Save();
        
        GoToMenu?.Invoke();
    }

#if UNITY_EDITOR
    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            StartCoroutine(StartBadEnding());
        }
    }
#endif
}