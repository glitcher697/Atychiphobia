using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.ActionSystem {
    public class IdleAction : MonoBehaviour, IAction {
        [SerializeField] private UnityEvent Event;

        public event Action ActionCompleted;
        public void StartAction(UnityAction<Results> onComplete) {
            if (Event != null) {
                Event.Invoke();
                ActionCompleted?.Invoke();
            }
        }
    }
}