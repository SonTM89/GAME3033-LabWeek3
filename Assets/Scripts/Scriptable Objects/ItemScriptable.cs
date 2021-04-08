using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemScriptable : ScriptableObject
{
    public string Name = "Item";
    public ItemCategory itemCategory = ItemCategory.None;
    public GameObject ItemPrefab;
    public bool Stackable;
    public int MaxStack;

    public int Amount => m_Amount;
    private int m_Amount = 1;

    public PlayerController controller { get; private set; }


    public virtual void Initialize(PlayerController _controller)
    {
        controller = _controller;
    }

    public abstract void UseItem(PlayerController _controller);

    public virtual void DeleteItem(PlayerController _controller)
    {

    }

    public virtual void DropItem(PlayerController _controller)
    {

    }

    public void ChangeAmount(int amount)
    {
        m_Amount += amount;
    }

    public void SetAmount(int amount)
    {
        m_Amount = amount;
    }
}


public enum ItemCategory
{
    None,
    Weapon,
    Consumable,
    Equipment,
    Ammo
}