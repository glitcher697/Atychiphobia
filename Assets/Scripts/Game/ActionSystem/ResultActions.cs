using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum Results {
    Success,
    HasNoItem,
    IndexIsTaken,
    MachineIsOn
}

public class ResultActions : MonoBehaviour {
    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private AudioSource errorSound;

    private bool isOn;
    
    public IEnumerator IndexIsTaken() {
        if (isOn) yield break;
        
        isOn = true;
        
        inventorySystem.ChangeCurrentInventoryCellColor(Color.red);
        
        yield return new WaitForSeconds(0.5f);
        inventorySystem.ChangeCurrentInventoryCellColor(Color.white);
        
        isOn = false;
    }

    public IEnumerator HasNoItem() {
        if (isOn) yield break;
        isOn = true;
        
        inventorySystem.ChangeAllInventoryCellsColor(Color.red);
        
        errorSound.Play();
        yield return new WaitForSeconds(errorSound.clip.length/2);
        inventorySystem.ChangeAllInventoryCellsColor(Color.white);
        
        isOn = false;
    }

    public IEnumerator MachineIsOn() {
        if (isOn) yield break;
        
        isOn = true;
        
        inventorySystem.ChangeCurrentInventoryCellColor(Color.red);
        
        errorSound.Play();
        yield return new WaitForSeconds(errorSound.clip.length/2);
        
        inventorySystem.ChangeCurrentInventoryCellColor(Color.white);

        isOn = false;
    }
}
