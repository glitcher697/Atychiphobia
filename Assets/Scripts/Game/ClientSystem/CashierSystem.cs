using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EntryPoint.EntryPoints;
using Game.ActionSystem;
using Game.GamePlay.Root;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CashierSystem : MonoBehaviour, IAction {
    [SerializeField] private AudioSource applySound;
    [SerializeField] private AudioSource checkSound;
    [SerializeField] private Animator animator;
    [SerializeField] private IdleAction doorsOpen;
    
    private ClientSystem _clientSystem;
    private InventorySystem _inventorySystem;
    private DaySystem _daySystem;
    private EndingEvent _endingEvent;

    private bool _doorsOpened = false;
    private bool _canWork = false;

    public IEnumerator<CashierSystem> Init( ClientSystem clientSystem, 
                                            InventorySystem inventorySystem,
                                            DaySystem daySystem,
                                            EndingEvent endingEvent) {
        _clientSystem = clientSystem;
        _daySystem = daySystem;
        _inventorySystem = inventorySystem;
        _endingEvent = endingEvent;

        doorsOpen.ActionCompleted += () => _doorsOpened = true;

        yield return this;
    }
    
    
    public void StartAction(UnityAction<Results> onComplete) {
        if(!GameplayEntryPoint.isSceneLoaded) return;

        if (_doorsOpened && !_canWork) {
            checkSound.Play();
            _daySystem.CashierActivated();
            _canWork = true;
            
        } else if(!_canWork) 
            return;

        var res = CheckOrder();

        int choice = PlayerPrefs.GetInt("Choice");
        
        if (res == Results.Success) {
            if (DaySystem.CurrentDay == 3 && _clientSystem.IsCurrentClientLast()) {
                if (choice == 1)
                    StartCoroutine(_endingEvent.StartEvent());
                else
                    StartCoroutine(_endingEvent.StartBadEnding());
            }
            else if (_clientSystem.IsCurrentClientLast()) 
                _daySystem.EndShift();
        }
        
        onComplete(res);
    }

    private Results CheckOrder() {
        Client currentClient = _clientSystem.GetCurrentClient();
        
        var inv = _inventorySystem.GetInventory();
        
        if (currentClient.order.All(a => inv.Contains(a))) {
            currentClient.hasClientItems = true;
            
            applySound.Play();
            animator.Play("cashier");

            foreach (var i in currentClient.order)
                _inventorySystem.RemoveItem(i);
            
            GameManager.money += currentClient.orderPrice;
            _clientSystem.NextStep();

            StartCoroutine(currentClient.GoToTable());
        }
        else 
            return Results.HasNoItem;

        return Results.Success;
    }
}
