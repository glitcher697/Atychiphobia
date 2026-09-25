using System;
using System.Collections.Generic;
using Game.ActionSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class DaySystem : MonoBehaviour {
    [Range(0,3)]
    [Tooltip("make it 0, if you wanna do as it must")]
    [SerializeField] private uint startDay;
    
    [Tooltip("make it 0, if you wanna do random")]
    [SerializeField] private uint countOfClients;

    [SerializeField] private GameObject blockCollider;

    [SerializeField] private IdleAction doorsOpen;

    [SerializeField] private string[] texts;
    [SerializeField] private TMP_Text notePadText; 
    
    private ComputerSystem _computerSystem;
    private QuestManager _questManager;
    
    public event Action StartSpawningClients;
    public event Action RestartScene;
    
    private static int _countOfClientsPerDay;
    private static int _currentDay = 1;

    public static int CountOfClientsPerDay => _countOfClientsPerDay;
    public static int CurrentDay => _currentDay;

    public IEnumerator<DaySystem> Init( ComputerSystem computerSystem,
                                        QuestManager questManager,
                                        InventorySystem inventorySystem,
                                        EventSystem eventSystem) {
        _computerSystem = computerSystem;
        _questManager = questManager;

        _computerSystem.enabled = false;

        if(startDay != 0)
            _currentDay = (int)startDay;
        else 
            _currentDay = PlayerPrefs.GetInt("CurrentDay");

#if UNITY_EDITOR
        if (_currentDay > 3)
            _currentDay = 1;
#endif        
        //Debug.Log(PlayerPrefs.GetInt("CurrentDay"));

        if (countOfClients == 0)
            _countOfClientsPerDay = 15 + Random.Range(-5,5);
        else
            _countOfClientsPerDay = (int)countOfClients;

        if (countOfClients > 40)
            countOfClients = 40;
        
        doorsOpen.ActionCompleted += () => {
            questManager.AddTask("Activate the cashier");
        };

        StartSpawningClients += () => {
            inventorySystem.ChangeAllInventoryCellsColor(Color.white);
            
            StartCoroutine(eventSystem.SpawnEvents());
        };
        
        _questManager.AddTask("Open the doors");

        notePadText.text = texts[_currentDay-1];
        
        yield return this;
    }

    public void AddDay() {
        _currentDay++;
        PlayerPrefs.SetInt("CurrentDay", _currentDay);
        PlayerPrefs.Save();
    }

    public void NextDay() {
        AddDay();
        
        RestartScene?.Invoke();
    }

    public void EndShift() {
        if(CurrentDay == 3) return;
        
        _computerSystem.enabled = true;
        _computerSystem.tag = "action";
        
        _questManager.AddTask("Make an upgrade in the PC");
    }
    
    public void CashierActivated() {
        _questManager.AddTask("Work!");
        _questManager.AddTask("Immediately.");
        StartCoroutine(_questManager.ResetQueue(5));
        
        StartSpawningClients?.Invoke();
        blockCollider.SetActive(true);
    }

#if UNITY_EDITOR
    private void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            NextDay();
    }
#endif
    
}
