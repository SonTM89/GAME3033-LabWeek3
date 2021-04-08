using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

namespace UI.Menus
{
    public class LoadGameWidget : MenuWidget
    {
        [SerializeField] private bool Debug;

        private const string SaveFileKey = "FileSaveData";

        private GameDataList gameData;

        [Header("Scene To load")]
        [SerializeField] private string SceneToLoad;

        [Header("Prefabs")]
        [SerializeField] private GameObject SaveSlotPrefab;

        [Header("References")]
        [SerializeField] private RectTransform LoadItemsPanel;
        [SerializeField] private TMP_InputField NewGameInputField;

        // Start is called before the first frame update
        void Start()
        {
            if (Debug) SaveDebugData();

            WipeChildren();

            LoadGameData();
        }

        private void WipeChildren()
        {
            foreach (RectTransform saveSlot in LoadItemsPanel)
            {
                Destroy(saveSlot.gameObject);
            }

            LoadItemsPanel.DetachChildren();
        }

        private void SaveDebugData()
        {
            GameDataList dataList = new GameDataList();
            dataList.SaveFileNames.AddRange(new List<string>{ "Save1",  "Save2", "Save3"});
            PlayerPrefs.SetString(SaveFileKey, JsonUtility.ToJson(dataList));
        }


        private void LoadGameData()
        {
            if (!PlayerPrefs.HasKey(SaveFileKey)) return;

            string jsonString = PlayerPrefs.GetString(SaveFileKey);
            gameData = JsonUtility.FromJson<GameDataList>(jsonString);

            if (gameData.SaveFileNames.Count <= 0) return;

            //UnityEngine.Debug.Log(gameData.SaveFileNames);

            foreach(string saveName in gameData.SaveFileNames)
            {
                SaveSlotWidget widget = Instantiate(SaveSlotPrefab, LoadItemsPanel).GetComponent<SaveSlotWidget>();
                widget.Initialize(this, saveName);
            }
        }


        public void LoadScene()
        {
            SceneManager.LoadScene(SceneToLoad);
        }


        public void CreateNewGame()
        {
            if (string.IsNullOrEmpty(NewGameInputField.text)) return;

            GameManager.Instance.SetActiveSave(NewGameInputField.text);

            LoadScene();
        }
    }


    [Serializable]
    class GameDataList
    {
        public List<string> SaveFileNames = new List<string>();
    }
}