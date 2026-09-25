using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
 
public class EventByLook : EventObject, IAction {
    private bool isOn;
    
    public override IEnumerator<EventObject> Init(EventEndingTypes endingType,
                                                    float duration) {
        
        this.endingType = endingType;
        this.duration = duration;
        
        yield return this;
    }
    
    public void StartAction(UnityAction<Results> onComplete) {  
        if(!isOn) return;
        eventDirector.Resume();
    }

    public override void StartEvent() {
        eventDirector.Play();
    }
    
    public override IEnumerator EndEvent() {
        eventDirector.Stop();
        yield break;
    }

    public void makeItOn() {
        isOn = true;
    }


}
