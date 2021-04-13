using Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUpComponent : MonoBehaviour
{
    [SerializeField] private ItemScriptable PickUpItem;

    [Tooltip("Manual Override for Drop Amount, if left at -1 it will use the amount from the scriptable object.")]
    [SerializeField] private int Amount = -1;
    
    [SerializeField] private MeshRenderer PropMeshRenderer;

    [SerializeField] private MeshFilter PropMeshFilter;

    private ItemScriptable ItemInstance;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate();
    }

    
    private void Instantiate()
    {
        ItemInstance = Instantiate(PickUpItem);

        if (Amount > 0) ItemInstance.SetAmount(Amount);

        ApplyMesh();
    }

    private void ApplyMesh()
    {
        if (PropMeshFilter) PropMeshFilter.mesh = PickUpItem.ItemPrefab.GetComponentInChildren<MeshFilter>().sharedMesh;
        if (PropMeshRenderer) PropMeshRenderer.materials = PickUpItem.ItemPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterials;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"{PickUpItem.name} - PickUp");

        //ItemInstance.UseItem(other.GetComponent<PlayerController>());

        //Destroy(gameObject);

        InventoryComponent playerInventory = other.GetComponent<InventoryComponent>();

        if(playerInventory)
        {
            playerInventory.AddItem(ItemInstance, Amount);

            Destroy(gameObject);
        }    
    }


    private void OnValidate()
    {
        ApplyMesh();
    }
}
