using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EntryPoint.EntryPoints;
using Game.GamePlay.Root;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class InventorySystem : MonoBehaviour {
    private List<InventoryItem> _inventory = new (9);
    
    [SerializeField] private Image[] inventoryCells = new Image[9];
    [SerializeField] private TMP_Text[] inventoryTitleCells;
    [SerializeField] private Sprite defaultSpite;
    
    private GameManager _gameManager;
    
    private int _selectedIndex = 1;
    private bool _isChangingColor = true;

    public IEnumerator<InventorySystem> Init(GameManager gameManager) {
        _gameManager = gameManager;
        
        yield return this;
    }

    public Results AddItem(TypeItems item) {
        foreach (var i in _inventory) {
            if (i.IndexInInventory == _selectedIndex) 
                return Results.IndexIsTaken;
        }
        
        InventoryItem invItem = new() {
            Item = _gameManager.GetItemByType(item),
            IndexInInventory = _selectedIndex
        };
        
        _inventory.Add(invItem);

        UpdateInventory();

        return Results.Success;
    }

    public void RemoveItem(TypeItems item) {
        
        InventoryItem remove_Item = new ();
        foreach (var i in _inventory) {
            if (i.Item.Type == item) {
                remove_Item = i;
                break;
            }
        }

        _inventory.Remove(remove_Item);
        
        UpdateInventory();
        
        ChangeAllInventoryCellsColor(Color.white);
        _isChangingColor = !_isChangingColor;
    }
    
    public List<TypeItems> GetInventory() {
        List<TypeItems> inv = new ();

        foreach (var i in _inventory)
            inv.Add(i.Item.Type);

        return inv;
    }

    public TypeItems GetCurrentItem() {
        TypeItems current = TypeItems.Null;
        foreach (var i in _inventory) {
            if (i.IndexInInventory == _selectedIndex) {
                current = i.Item.Type;
                break;
            }
        }

        return current;
    }

    public Image GetCurrentInventoryCell() {
        return inventoryCells[_selectedIndex];
    }

    public void ChangeCurrentInventoryCellColor(Color color) {
        _isChangingColor = !_isChangingColor;
        GetCurrentInventoryCell().color = color;
    }
    
    public void ChangeAllInventoryCellsColor(Color color) {
        _isChangingColor = !_isChangingColor;
        foreach (var i in inventoryCells) {
            i.color = color;
        }
    }
    
    void Update() {
        if(!GameplayEntryPoint.isSceneLoaded) return;
        
        if(_isChangingColor) return;
        
        inventoryCells[_selectedIndex].color = Color.white;

        _selectedIndex += Input.GetAxis("Mouse ScrollWheel") > 0 ? 0 : 1;
        _selectedIndex -= Input.GetAxis("Mouse ScrollWheel") < 0 ? 0 : 1;
        
        if (_selectedIndex == -1)
            _selectedIndex = inventoryCells.Length-1;
        
        if (_selectedIndex == inventoryCells.Length)
            _selectedIndex = 0;
        
        if (_isChangingColor) return;
        
        inventoryCells[_selectedIndex].color = Color.gold;
    }

    void UpdateInventory() {
        foreach (var j in inventoryCells)
            j.sprite = defaultSpite;
        foreach (var i in inventoryTitleCells) 
            i.text = "";
        
        foreach (var i in _inventory) {
            inventoryCells[i.IndexInInventory].sprite = i.Item.Img;
            inventoryTitleCells[i.IndexInInventory].text = i.Item.Type.ToString();
        }
    }
}
