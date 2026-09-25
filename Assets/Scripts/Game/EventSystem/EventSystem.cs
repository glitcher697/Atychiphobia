using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EventSystem : MonoBehaviour {
    [SerializeField] private List<Event> events;
    [SerializeField] private float timeBetweenEvents;

    private Queue<Event> _eventQueue = new Queue<Event>();
    
    public IEnumerator SpawnEvents() {
        yield return new WaitForSeconds(Random.Range(15, 25));
        
        foreach (var currentEvent in _eventQueue) {
            currentEvent.EventObject.EventDirector.gameObject.SetActive(true);
        
            currentEvent.EventObject.Init(currentEvent.EventEndingTypes, currentEvent.EventDuration);
        
            currentEvent.EventObject.StartEvent();
            
            yield return new WaitForSeconds(timeBetweenEvents + Random.Range(-5,5));

#if  UNITY_EDITOR
            Debug.Log(currentEvent.EventObject.gameObject.name);
#endif
        }
    }
    
    public IEnumerator<EventSystem> Init() {
        //random queue filling
        var eventList = events.OrderBy(x => Guid.NewGuid().ToString()).ToList();
        
        foreach (var i in eventList) {
            if(i.Day >= DaySystem.CurrentDay)
                _eventQueue.Enqueue(i);
            
            i.EventObject.Init(i.EventEndingTypes, i.EventDuration);
        }
        
        yield return this;
    }
    
    
}
