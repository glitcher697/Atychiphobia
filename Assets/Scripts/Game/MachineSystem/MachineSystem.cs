using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MachineSystem : MonoBehaviour {
    [SerializeField] protected TypeItems item; 
    
    protected GameManager _gameManager;
    protected InventorySystem _inventory;

    //public void StartAction(UnityAction<ActionResults> onComplete) { }

    public virtual IEnumerator<MachineSystem> Init(InventorySystem inventorySystem, GameManager gameManager) {
        _inventory = inventorySystem;
        _gameManager = gameManager;
        yield return this;
    }
}
