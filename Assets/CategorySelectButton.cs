using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategorySelectButton : MonoBehaviour
{
    [SerializeField] private ItemCategory category;

    // References
    private Button SelectButton;
    private InventoryWidget inventoryWidget;


    private void Awake()
    {
        SelectButton = GetComponent<Button>();
        SelectButton.onClick.AddListener(OnClick);
    }


    public void Initialize(InventoryWidget _inventoryWidget)
    {
        inventoryWidget = _inventoryWidget;
    }


    private void OnClick()
    {
        if (!inventoryWidget) return;

        inventoryWidget.SelectCategory(category);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}