using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class IconSlot : MonoBehaviour
{
    private ItemScriptable Item;

    private Button ItemButton;

    private TMP_Text ItemText;

    [SerializeField] private ItemSlotAmountWidget AmountWidget;

    [SerializeField] private ItemSlotEquippedWidget EquippedWidget;


    private void Awake()
    {
        ItemButton = GetComponent<Button>();
        ItemText = GetComponentInChildren<TMP_Text>();

        //AmountWidget = GetComponentInChildren<ItemSlotAmountWidget>();
        //EquippedWidget = GetComponentInChildren<ItemSlotEquippedWidget>();
    }


    public void Initialize(ItemScriptable item)
    {
        Item = item;
        ItemText.text = item.Name;

        AmountWidget.Initialize(item);
        EquippedWidget.Initialize(item);

        ItemButton.onClick.AddListener(UseItem);
        Item.OnItemDestroyed += OnItemDestroyed;
    }


    public void UseItem()
    {
        Debug.Log($"{Item.Name} - Item Used");

        Item.UseItem(Item.controller);
    }


    private void OnItemDestroyed()
    {
        Item = null;
        Destroy(gameObject);
    }


    private void OnDisable()
    {
        if(Item) Item.OnItemDestroyed -= OnItemDestroyed;
    }
}
