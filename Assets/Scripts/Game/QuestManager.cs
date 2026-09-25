using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    [SerializeField] private TMP_Text taskText;
    [SerializeField] private AudioSource newTaskSound;

    private Queue<string> _queueMessages = new Queue<string>();

    public IEnumerator<QuestManager> Init() {
        StartCoroutine(QueueUpdating());
        yield return this;
    }
    
    public void AddTask(string text) {
        _queueMessages.Enqueue(text);
    }

    public IEnumerator ResetQueue(int timeToLastTask) {
        yield return new WaitForSeconds(_queueMessages.Count*3+timeToLastTask);
        _queueMessages.Clear();
        taskText.text = "";
    }

    private IEnumerator QueueUpdating() {
        Debug.Log(_queueMessages.Count);
        while (true){
            if(_queueMessages.Count > 0 ){
                taskText.text = _queueMessages.Dequeue();
                newTaskSound.Play();
                yield return new WaitForSeconds(3);
            }

            yield return new WaitForSeconds(1);
        }
    }
}