using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EntryPoint;
using EntryPoint.EntryPoints;
using Game.GamePlay.Root;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    [SerializeField] private List<Item> items;
    [SerializeField] private TMP_Text allMoneyText;

    [SerializeField] private FirstPersonLook FPL;
    [SerializeField] private FirstPersonMovement FPM;

    [SerializeField] private AudioSource[] audioSources;

    [SerializeField] private GameObject UI;
    
    public static int money;

    [CanBeNull] private static GameManager _instance;
    [CanBeNull] public static GameManager Instance => _instance;
    
    private ActionSystem _actionSystem;
    
    public IEnumerator<GameManager> Init(ActionSystem actionSystem) {
        _instance = this;
        _actionSystem = actionSystem;
        
        GetChangePlayerActions(true);
        UnPauseMusic();
        ShowUI();
        
        yield return this;
    }

    public Item GetItemByType(TypeItems type) {
        foreach (var i in items) {
            if (type == i.Type) 
                return i;
        }

        return null;
    }

    public List<TypeItems> GetAllItemTypes() {
        List<TypeItems> temp = new List<TypeItems>();
        foreach (var i in items) 
            temp.Add(i.Type);

        return temp;
    }
    
    public List<Item> GetAllItems() {
        List<Item> temp = new List<Item>();
        foreach (var i in items) 
            temp.Add(i);

        return temp;
    }

    public int GetLvlByType(TypeItems type) {
        
        foreach (var i in items) {
            if (type == i.Type) {
                return i.Lvl;
            }
        }
        return 1;
    }

    public void GetChangePlayerActions(bool isOn) {
        Rigidbody rigidbodyFpm = FPM.GetComponent<Rigidbody>();
        
        if(!isOn) {
            rigidbodyFpm.Sleep();
            rigidbodyFpm.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else {
            rigidbodyFpm.WakeUp();
            rigidbodyFpm.constraints = RigidbodyConstraints.FreezeRotationX |
                                       RigidbodyConstraints.FreezeRotationZ;
        }
        
        FPM.enabled = isOn;
        FPL.enabled = isOn;
        _actionSystem.enabled = isOn;
        Cursor.visible = !isOn;
        Cursor.lockState = isOn ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void ChangeCursor(bool isOn) {
        Cursor.visible = isOn;
        Cursor.lockState = !isOn ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void PauseMusic(){
        foreach (var i in audioSources)
            i.Pause();
    }
    
    public void UnPauseMusic() {
        foreach (var i in audioSources)
            i.UnPause();
    }

    public void HideUI() => UI.SetActive(false);

    public void ShowUI() => UI.SetActive(true);
    
    private void Update() {
        if(!GameplayEntryPoint.isSceneLoaded) return;
        
        allMoneyText.text = money + "$";
    }
}
