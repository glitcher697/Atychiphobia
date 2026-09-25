using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class nightmareEvent : MonoBehaviour {
    [SerializeField] private PlayableDirector director;
    
    private GameManager _gameManager;
    private ComputerSystem _computerSystem;
    private DaySystem _daySystem;
    
    public event Action GoToNightmare;

    public IEnumerator<nightmareEvent> Init(GameManager gameManager, 
                                            ComputerSystem computerSystem,
                                            DaySystem daySystem) {
        _gameManager = gameManager;
        _computerSystem = computerSystem;
        _daySystem = daySystem;

        yield return this;
    }
    
    public void StartEvent() {
        if (DaySystem.CurrentDay != 2) {
            _daySystem.NextDay();
            return;
        }
        
        _daySystem.AddDay();
        _computerSystem.GetOffScreen();
        _gameManager.GetChangePlayerActions(false);
        _gameManager.PauseMusic();
        
        director.Play();
    }

    public void LoadNightmare() {
        GoToNightmare?.Invoke();
    }
    
#if UNITY_EDITOR
    private void Update() {
        if(Input.GetKeyDown(KeyCode.J)) {
            _gameManager.GetChangePlayerActions(false);
            StartEvent();
        }
    }
#endif
}
