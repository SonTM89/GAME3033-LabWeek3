using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UI.Menus;
using System;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private GameSaveData GameSave;

    private const string FileSaveKey = "FileSaveData";


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        if (string.IsNullOrEmpty(GameManager.Instance.GameSaveName)) return;

        if (!PlayerPrefs.HasKey(GameManager.Instance.GameSaveName)) return;

        string jSonString = PlayerPrefs.GetString(GameManager.Instance.GameSaveName);
        GameSave = JsonUtility.FromJson<GameSaveData>(jSonString);

        //LoadGame();
    }


    public void SaveGame()
    {
        GameSave ??= new GameSaveData();

        var savableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>().ToList();

        Debug.Log("Working");

        ISavable playerSaveData = savableObjects.First(monoObject => monoObject is PlayerController);

        GameSave.playerSaveData = playerSaveData?.SaveData() as PlayerSaveData;

        string saveDataString = JsonUtility.ToJson(GameSave);
        PlayerPrefs.SetString(GameManager.Instance.GameSaveName, saveDataString);

        SaveToFileList();
    }


    public void LoadGame()
    {
        var savableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>().ToList();

        ISavable playerSaveObj = savableObjects.First(monoObject => monoObject is PlayerController);

        playerSaveObj?.LoadData(GameSave.playerSaveData);
    }


    public void SaveToFileList()
    {
        if (PlayerPrefs.HasKey(FileSaveKey))
        {
            GameDataList dataList = JsonUtility.FromJson<GameDataList>(PlayerPrefs.GetString(FileSaveKey));

            if (dataList.SaveFileNames.Contains(GameManager.Instance.GameSaveName)) return;
            dataList.SaveFileNames.Add(GameManager.Instance.GameSaveName);

            PlayerPrefs.SetString(FileSaveKey, JsonUtility.ToJson(dataList));
        }

        else
        {
            GameDataList data = new GameDataList();
            data.SaveFileNames.Add(GameManager.Instance.GameSaveName);

            PlayerPrefs.SetString(FileSaveKey, JsonUtility.ToJson(data));
        }
    }
}


[Serializable]
public class GameSaveData
{
    public PlayerSaveData playerSaveData;

    public GameSaveData()
    {
        playerSaveData = new PlayerSaveData();
    }
}