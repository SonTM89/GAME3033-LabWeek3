using Character;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWidget : GameHudWidget
{
    private ItemDisplayPanel itemDisplayPanel;

    private List<CategorySelectButton> CategoryButtons;

    private PlayerController playerController;


    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        CategoryButtons = GetComponentsInChildren<CategorySelectButton>().ToList();
        itemDisplayPanel = GetComponentInChildren<ItemDisplayPanel>();

        foreach(CategorySelectButton button in CategoryButtons)
        {
            button.Initialize(this);
        }
    }


    private void OnEnable()
    {
        if (!playerController || !playerController.Inventory) return;

        if (playerController.Inventory.GetItemCount() <= 0) return;

        itemDisplayPanel.PopulatePanel(playerController.Inventory.GetItemsOfCategory(ItemCategory.None));
    }


    public void SelectCategory(ItemCategory category)
    {
        if (!playerController || !playerController.Inventory) return;

        if (playerController.Inventory.GetItemCount() <= 0) return;

        itemDisplayPanel.PopulatePanel(playerController.Inventory.GetItemsOfCategory(category));
    }
}
