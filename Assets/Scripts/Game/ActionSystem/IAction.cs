using UnityEngine;
using UnityEngine.Events;

public interface IAction {
    public void StartAction(UnityAction<Results> onComplete);
}
