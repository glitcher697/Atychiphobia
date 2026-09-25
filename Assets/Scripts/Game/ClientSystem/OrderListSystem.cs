using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OrderListSystem : MonoBehaviour {
    [SerializeField] private SpriteRenderer[] itemCells;
    [SerializeField] private TMP_Text[] titles;
    [SerializeField] private TMP_Text orderPrice;
    [SerializeField] private Slider slider;
    
    private GameManager _gameManager;
    
    private Sprite _sprite;
    
    private Queue<List<TypeItems>> ShowItemsQueue = new ();

    public static bool isTimeOver;

    public IEnumerator<OrderListSystem> Init(GameManager gameManager) {
        _gameManager = gameManager;
        yield return this;
    }

    private void ShowItems() {
        if(ShowItemsQueue.Count == 0) return;
        
        foreach (var g in itemCells) 
            g.sprite = null;
        
        foreach (var v in titles)
            v.text = "";

        int j = 0;
        
        foreach (var i in ShowItemsQueue.Peek()) {
            _sprite = _gameManager.GetItemByType(i).Img;

            if (_sprite != null) {
                itemCells[j].sprite = _sprite;
                titles[j].text = i.ToString();
                j++;
            }
        }
    }

    public void AddOnBoard(List<TypeItems> toEnqueue, Client currentClient) {
        ShowItemsQueue.Enqueue(toEnqueue);
        orderPrice.text = currentClient.orderPrice + "$";
        
        ShowItems();
    }
    public void RemoveFromBoard() {
        ShowItemsQueue.Dequeue();
        ShowItems();
    }
    
}
