using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Menus;
using TMPro;

public class SaveSlotWidget : MonoBehaviour
{
    private string SaveName;

    private GameManager manager;

    private LoadGameWidget LoadWidget;

    [SerializeField] private TMP_Text SaveNameText;


    private void Awake()
    {
        manager = GameManager.Instance;

    }


    public void Initialize(LoadGameWidget parentWidget, string saveName)
    {
        LoadWidget = parentWidget;
        SaveName = saveName;
        SaveNameText.text = saveName;
    }


    public void SelectSave()
    {
        manager.SetActiveSave(SaveName);
        LoadWidget.LoadScene();
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
