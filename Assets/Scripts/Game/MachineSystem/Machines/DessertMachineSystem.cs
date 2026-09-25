using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DessertMachineSystem : MachineSystem, IAction {
    public void StartAction(UnityAction<Results> onComplete) {
        Results results = _inventory.AddItem(item);
        onComplete(results);
    }

    public override IEnumerator<MachineSystem> Init(InventorySystem inventorySystem, GameManager gameManager) {
        _inventory = inventorySystem;
        _gameManager = gameManager;
        
        if(_gameManager.GetLvlByType(item)> DaySystem.CurrentDay)
            gameObject.SetActive(false);

        yield return this;
    }
}
 