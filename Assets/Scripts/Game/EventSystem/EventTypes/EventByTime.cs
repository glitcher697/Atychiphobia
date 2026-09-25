using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EventByTime : EventObject {
    [Header("Settings")] 
    [Tooltip("It's time between the real ending of event and starting ending in the code")]
    [SerializeField] private int timeToEnd = 5;
    
    public override IEnumerator<EventObject> Init(EventEndingTypes endingType,
                                                  float duration) {
        
        this.endingType = endingType;
        this.duration = duration;
        
        yield return this;
    }

    public override void StartEvent() {
        eventDirector.Play();
        
        StartCoroutine(EndEvent());
    }

    public override IEnumerator EndEvent() {
        yield return new WaitForSeconds(timeToEnd + duration);
        
        eventDirector.Stop();
    }
}
