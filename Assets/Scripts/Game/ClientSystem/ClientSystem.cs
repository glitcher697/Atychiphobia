using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ClientSystem : MonoBehaviour {
    [SerializeField] private Transform[] points;
    
    [SerializeField] private GameObject[] prefabClients;
    [SerializeField] private Transform orderPoint, startPoint;

    [SerializeField] private float startRemainDistance;
    [SerializeField] [Range(1,6)] private int maxCountOfItems;

    [SerializeField] private GameObject parentObject;
    
    private OrderListSystem _orderListSystem;
    private GameManager _gameManager;
    private DaySystem _daySystem;

    private Queue<Client> _clients = new ();

    public IEnumerator Init(OrderListSystem orderListSystem, 
                            GameManager gameManager, 
                            DaySystem daySystem) {
        
        _orderListSystem = orderListSystem;
        _gameManager = gameManager;
        _daySystem = daySystem;

        _daySystem.StartSpawningClients += () => 
            StartCoroutine(CreatingClients());
        
        yield return null;
    }
    
    public void NextStep() {
        _clients.Dequeue();
        _orderListSystem.RemoveFromBoard();
        
        float step = 0;
        foreach (var i in _clients) {
            i.remainDistance = startRemainDistance + step;
            step++;
            
            i.SetDestination(orderPoint);
        }
    }

    public Client GetCurrentClient() {
        return _clients.Peek();
    }

    public void HideAllMeshClients() {
        foreach (var i in _clients) 
            i.HideMesh();
    }

    public void ShopAllMeshClients() {
        foreach (var i in _clients) {
            i.ShowMesh();
        }
    }

    public bool IsCurrentClientLast() {
        return _clients.Count <= 1;
    }
    
    private IEnumerator CreatingClients() {
        int step = 0;
        for (int i = 0; i <= DaySystem.CountOfClientsPerDay; i++) {
            
            if (_clients.Count >= 4) { // Waits if there are more than 4 clients in the group
                i--;
                yield return new WaitForSeconds(1);
                continue;
            }
            
            if (step == 6) step = 0; /// This is necessary so that from
                                     /// the initial point "startRemainDistance" the "Client"
                                    /// moves away by 1 step, this is creating a queue///
            
            var prefabClient = Instantiate(
                prefabClients[Random.Range(0, prefabClients.Length)], parentObject.transform, false); //Creating a model
            
            prefabClient.transform.position = startPoint.position;
            
            Client client = prefabClient.GetComponent<Client>();
            
            List<TypeItems> order = MakeOrder();
            
            client.Init(
                table: points[Random.Range(1, points.Length)],
                order: order,
                remainDistance: step+startRemainDistance, 
                orderPrice: Random.Range(1,5) * order.Count,
                getOutPoint: startPoint
            ); // Get params
            
            step++;

            client.SetDestination(orderPoint);
            
            _clients.Enqueue(client);
            _orderListSystem.AddOnBoard(order, client); // To Show on order list
            
            yield return new WaitForSeconds(1);
        }
    }
    
    private List<TypeItems> MakeOrder() {
        List<TypeItems> order = new ();

        List<Item> allItems = _gameManager.GetAllItems();
        
        foreach (var j in allItems) {
            if(j.Lvl > DaySystem.CurrentDay) 
                break;
            
            if(order.Count == maxCountOfItems) 
                break;
            
            if (Random.Range(1, 3) == 1) 
                order.Add(j.Type);
        }
        
        if(order.Count == 0)
            order.Add(allItems[0].Type);

        return order;
    }
    
}
