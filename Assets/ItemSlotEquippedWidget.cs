using Scriptable_Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotEquippedWidget : MonoBehaviour
{
    private EquippableScriptable Equippable;

    [SerializeField] private Image Enabledimage;


    private void Awake()
    {
        HideWidget();
    }


    public void ShowWidget()
    {
        gameObject.SetActive(true);
    }


    public void HideWidget()
    {
        gameObject.SetActive(false);
    }


    public void Initialize(ItemScriptable item)
    {
        if (!(item is EquippableScriptable eqItem)) return;

        Equippable = eqItem;

        ShowWidget();

        Equippable.OnEquipStatusChange += OnEquipmentChange;

        OnEquipmentChange();
    }

    private void OnEquipmentChange()
    {
        Enabledimage.gameObject.SetActive(Equippable.Equipped); 
    }


    private void OnDisable()
    {
        if(Equippable) Equippable.OnEquipStatusChange -= OnEquipmentChange;
    }
}
