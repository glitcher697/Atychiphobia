using System;
using UnityEngine;

[Serializable]
public enum EventEndingTypes {
    ByLook,
    ByTime
}

[Serializable]
public class Event {
    [SerializeField] private EventObject eventObject;
    [SerializeField] private EventEndingTypes eventEndingTypes;
    [SerializeField] private float eventDuration;
    [Range(1,3)] [SerializeField] private int day;
    public EventObject EventObject => eventObject;
    public EventEndingTypes EventEndingTypes => eventEndingTypes;
    public float EventDuration => eventDuration;
    public int Day => day;
}