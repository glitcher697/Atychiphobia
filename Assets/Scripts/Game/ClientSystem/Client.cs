using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Client : MonoBehaviour {
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject mesh;

    [SerializeField] private float numberToUp;
    private Transform _table, _getOutPoint;
    
    public float remainDistance;
    public List<TypeItems> order;
    public bool hasClientItems;
    public int orderPrice;
    
    private void Update() {
        if(!navMeshAgent.enabled) return;
        
        if(navMeshAgent.pathPending) return;
        
        if (navMeshAgent.remainingDistance < remainDistance) {
            navMeshAgent.enabled = false;
            
            if (hasClientItems) {
                transform.position = new Vector3(
                    _table.transform.position.x,
                    _table.transform.position.y + numberToUp,
                    _table.transform.position.z
                );
            }
        }
    }

    public void Init(Transform table,
                            List<TypeItems> order,
                            float remainDistance,
                            int orderPrice,
                            Transform getOutPoint) {
        _table = table;
        _getOutPoint = getOutPoint;
        
        this.order = order;
        this.remainDistance = remainDistance;
        this.orderPrice = orderPrice;
    }

    public void SetDestination(Transform point) {
        navMeshAgent.enabled = true;
        
        navMeshAgent.ResetPath();
        navMeshAgent.SetDestination(point.position);
    }
    
    public IEnumerator GoToTable() {
        SetDestination(_table);
        
        yield return new WaitForSeconds(10); //10
        
        SetDestination(_getOutPoint);

        yield return new WaitForSeconds(5);
        
        Destroy(gameObject);
    }

    public void HideMesh() {
        mesh.SetActive(false);
    }

    public void ShowMesh() {
        mesh.SetActive(true);
    }
}
