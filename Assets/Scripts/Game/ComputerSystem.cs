using System;
using System.Collections.Generic;
using System.Linq;
using EntryPoint.EntryPoints;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ComputerSystem : MonoBehaviour, IAction {
    
    [SerializeField] private GameObject PC_Canvas , PC_Volume;
    [SerializeField] private GameObject Main_Canvas , Main_Volume;

    [SerializeField] private TMP_Text lvlText;
    [SerializeField] private Button upgradeBut;
    [SerializeField] private GameObject endBut;
    [SerializeField] private GameObject exitBut;
    [SerializeField] private int startLvl=1;
    
    private GameManager _gameManager;

    private static int _lvl = 1;
    public static int Lvl => _lvl;
    
    public IEnumerator<ComputerSystem> Init(GameManager gameManager) {
        _gameManager = gameManager;
        _lvl = startLvl;
        yield return this;
    }
    
    public void StartAction(UnityAction<Results> onComplete) {
        Main_Volume.SetActive(false);
        Main_Canvas.SetActive(false);
        PC_Canvas.SetActive(true);
        PC_Volume.SetActive(true);

        _gameManager.GetChangePlayerActions(false);
    }

    public void GetOffScreen() {
        Main_Volume.SetActive(true);
        Main_Canvas.SetActive(true);
        PC_Canvas.SetActive(false);
        PC_Volume.SetActive(false);

        _gameManager.GetChangePlayerActions(true);
    }

    public void Upgrade() {
        if(!GameplayEntryPoint.isSceneLoaded) return;
        
        lvlText.text = "lvl: " + Lvl;
        
        upgradeBut.interactable = false;
        // playing an audio...
        endBut.SetActive(true);
        exitBut.SetActive(false);
        
    }
}
