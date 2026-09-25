using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CoffeeMachineSystem : MachineSystem, IAction {
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private GameObject cup;
    [SerializeField] private TMP_Text timerTxt;
    [SerializeField] private AudioSource sound;
    [SerializeField] private TMP_Text text;

    [SerializeField] private float duration;

    private int _tempTime;
    private bool _isOn, _isComplete;
    
    public override IEnumerator<MachineSystem> Init(InventorySystem inventorySystem, GameManager gameManager) {
        _inventory = inventorySystem;
        _gameManager = gameManager;
        
        var particles = particleSystem.main;
        particles.duration = duration;
        
        text.text = item.ToString();
        
        if(_gameManager.GetLvlByType(item)> DaySystem.CurrentDay)
            gameObject.SetActive(false);

        yield return this;
    }
    
    public void StartAction(UnityAction<Results> onComplete) {
        
        if (_isOn) {
            onComplete(Results.MachineIsOn);
            return;
        }

        if (_isComplete) {
            Results results = _inventory.AddItem(item);

            if (results != Results.Success) {
                onComplete(results);
                return;
            }
            
            cup.SetActive(false);
            _isComplete = false;
            
            onComplete(results);
            return;
        }
        
        sound.Play();
        
        _isOn = true;
        _tempTime = (int)Math.Ceiling(duration);
        cup.SetActive(true);
        particleSystem.Play();
        StartCoroutine(Timer());
    }

    private IEnumerator Timer() {
        timerTxt.text = _tempTime.ToString();
        yield return new WaitForSeconds(1);
        _tempTime--;

        if (_tempTime == 0) {
            _isOn = false;
            _isComplete = true;
            timerTxt.text = "";
            
            yield break;
        }

        StartCoroutine(Timer());
    }
    
}
