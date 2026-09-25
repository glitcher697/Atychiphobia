using System;
using UnityEngine;

[Serializable]
public enum TypeItems
{
    Cap,
    Latte,
    Lungo,
    Flat,
    Filter,
    Raf,
    Bread,
    Milk,
    PinkMacaron,
    BlueMacaron,
    YellowMacaron,
    CreamPuff,
    Null
}

[Serializable]
public class Item {
    [SerializeField] private TypeItems type; 
    [SerializeField] private Sprite img;
    [SerializeField] private int lvl;

    public TypeItems Type => type;
    public Sprite Img => img;
    public int Lvl => lvl;
}

[Serializable]
public class InventoryItem {
    public Item Item;
    public int IndexInInventory;
}