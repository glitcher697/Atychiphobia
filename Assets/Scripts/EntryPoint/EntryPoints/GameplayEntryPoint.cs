using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;

namespace EntryPoint.EntryPoints {
    public class GameplayEntryPoint : EntryPoint {
        [SerializeField] private ResultActions resultActions;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private ClientSystem clientSystem;
        [SerializeField] private OrderListSystem orderListSystem;
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private DaySystem daySystem;
        [SerializeField] private ActionSystem actionSystem;
        [SerializeField] private ComputerSystem computerSystem;
        [SerializeField] private CashierSystem cashierSystem;
        [FormerlySerializedAs("taskManager")] [SerializeField] private QuestManager questManager;
        
        [SerializeField] private MachineSystem[] machineSystems;
        
        [SerializeField] private nightmareEvent nightmareEvent;
        [SerializeField] private EndingEvent endingEvent;

        public override bool _isSceneLoaded {
            get => isSceneLoaded;
            protected set => isSceneLoaded = value;
        }

        public static bool isSceneLoaded { get; protected set; }
        
        public event Action GoToMainMenuScene;
        public event Action RestartScene;
        public event Action GoToNightmareScene;

        private UIRootView _uiRootView;

        public IEnumerator Init(UIRootView uiRootView) {
            _uiRootView = uiRootView;
            yield return null;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public override IEnumerator Run() {
            yield return actionSystem.Init(resultActions);
            yield return gameManager.Init(actionSystem);
            yield return eventSystem.Init();
            yield return questManager.Init();
            yield return computerSystem.Init(gameManager);
            yield return orderListSystem.Init(gameManager);
            yield return inventorySystem.Init(gameManager);
            yield return nightmareEvent.Init(gameManager, computerSystem, daySystem);
            yield return endingEvent.Init(gameManager);
            yield return daySystem.Init(computerSystem, questManager, inventorySystem, eventSystem);
            yield return clientSystem.Init(orderListSystem, gameManager, daySystem);
            yield return cashierSystem.Init(clientSystem, inventorySystem, daySystem, endingEvent);

            foreach (var i in machineSystems) 
                yield return i.Init(inventorySystem, gameManager);
            
            nightmareEvent.GoToNightmare += () => GoToNightmareScene?.Invoke();

            daySystem.RestartScene += () => RestartScene?.Invoke();

            _uiRootView.GoToMainMenu += () => GoToMainMenuScene?.Invoke();

            endingEvent.GoToMenu += () => GoToMainMenuScene?.Invoke();

            _isSceneLoaded = true;

            yield return null;
        }
    }
}