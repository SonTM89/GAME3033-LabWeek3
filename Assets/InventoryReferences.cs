using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryReferences : MonoBehaviour
{
    public static InventoryReferences Instance;

    [SerializeField] private List<ItemScriptable> ItemList = new List<ItemScriptable>();

    private readonly Dictionary<string, ItemScriptable> ItemDictionary = new Dictionary<string, ItemScriptable>();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        foreach(ItemScriptable item in ItemList)
        {
            ItemDictionary.Add(item.Name, item);
        }
    }


    public ItemScriptable GetItemReferance(string itemName) =>
        ItemDictionary.ContainsKey(itemName) ? ItemDictionary[itemName] : null;
}
