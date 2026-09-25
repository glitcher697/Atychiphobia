using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public abstract class EventObject : MonoBehaviour { 
    [SerializeField] protected PlayableDirector eventDirector;
    [Tooltip("If the controller only has one clip, you can use only that one. You only need to fill in the 'startAnimationName' field.")]
    protected EventEndingTypes endingType;
    protected float duration;

    public EventEndingTypes EndingType => endingType;
    public PlayableDirector EventDirector => eventDirector;

    public abstract IEnumerator<EventObject> Init(EventEndingTypes endingType, float duration);

    public abstract void StartEvent();
    public abstract IEnumerator EndEvent();
}
