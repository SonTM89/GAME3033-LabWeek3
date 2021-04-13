using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.UI;
using Systems.Health;
using UI.Menus;
using UnityEngine.InputSystem;
using System;
using Scriptable_Objects;
using System.Linq;

namespace Character
{
    [RequireComponent(typeof(PlayerHealthComponent))]
    public class PlayerController : MonoBehaviour, IPausable, ISavable
    {
        public CrossHairScript CrossHair => CrossHairComponent;
        [SerializeField]
        private CrossHairScript CrossHairComponent;

        public bool IsFiring;
        public bool IsReloading;
        public bool IsJumping;
        public bool IsRunning;
        public bool InInventory;

        public HealthComponent Health => healthComponent;
        private HealthComponent healthComponent;

        public InventoryComponent Inventory => InventoryComponent;
        private InventoryComponent InventoryComponent;

        public WeaponHolder WeaponHolder => weaponHolderComponent;
        private WeaponHolder weaponHolderComponent;

        private GameUIController uIController;

        private PlayerInput playerInput;


        private void Awake()
        {
            uIController = FindObjectOfType<GameUIController>();
            playerInput = GetComponent<PlayerInput>();

            if (healthComponent == null) healthComponent = GetComponent<HealthComponent>();
            if (weaponHolderComponent == null) weaponHolderComponent = GetComponent<WeaponHolder>();
            if (InventoryComponent == null) InventoryComponent = GetComponent<InventoryComponent>();
        }


        public void OnPauseGame(InputValue value)
        {
            Debug.Log("Pause Game");
            PauseManager.Instance.PauseGame();
        }


        public void OnUnPauseGame(InputValue value)
        {
            Debug.Log("UnPause Game");
            PauseManager.Instance.UnpauseGame();
        }


        public void OnInventory(InputValue button)
        {
            if (InInventory)
            {
                InInventory = false;

                OpenIvnentory(false);
            }
            else
            {
                InInventory = true;

                OpenIvnentory(true);
            }
        }

        private void OpenIvnentory(bool open)
        {
            if (open)
            {
                PauseManager.Instance.PauseGame();
                uIController.EnableInventoryMenu();
            }
            else
            {
                PauseManager.Instance.UnpauseGame();
                uIController.EnableGameMenu();
            }
        }

        void IPausable.PauseMenu()
        {
            uIController.EnablePauseMenu();
            playerInput.SwitchCurrentActionMap("PauseActionMap");
        }


        void IPausable.UnPauseMenu()
        {
            uIController.EnableGameMenu();
            playerInput.SwitchCurrentActionMap("PlayerActionMap");
        }


        public void OnSaveGame(InputValue button)
        {
            SaveSystem.Instance.SaveGame();
        }


        public void OnLoadGame(InputValue button)
        {
            SaveSystem.Instance.LoadGame();
        }


        public SaveDataBase SaveData()
        {
            Transform playerTransform = transform;

            PlayerSaveData saveData = new PlayerSaveData
            {
                Name = gameObject.name,
                CurrentHealth = Health.Health,
                Position = playerTransform.position,
                Rotation = playerTransform.rotation
            };

            List<ItemSaveData> ItemSaveList = Inventory.GetItemList().Select(
                item => new ItemSaveData(item)).ToList();

            saveData.itemList = ItemSaveList;

            saveData.EquippedWeapon = (!WeaponHolder.EquippedWeapon) ? null : new WeaponSaveData(WeaponHolder.EquippedWeapon.WeaponInformation);

            return saveData;
        }

        public void LoadData(SaveDataBase saveData)
        {
            PlayerSaveData playerData = (PlayerSaveData)saveData;

            if (playerData == null) return;

            Transform playerTransform = transform;
            playerTransform.position = playerData.Position;
            playerTransform.rotation = playerData.Rotation;

            Health.SetCurrentHealth(playerData.CurrentHealth);

            foreach(ItemSaveData itemSaveData in playerData.itemList)
            {
                ItemScriptable item = InventoryReferences.Instance.GetItemReference(itemSaveData.Name);
                Inventory.AddItem(item, itemSaveData.Amount);
            }

            if (playerData.EquippedWeapon == null) return;

            WeaponScriptable weapon = (WeaponScriptable)Inventory.FindItem(playerData.EquippedWeapon.Name);

            if (!weapon) return;

            weapon.weaponStats = playerData.EquippedWeapon.weaponStats;
            WeaponHolder.EquipWeapon(weapon);
        }
    }



    [Serializable]
    public class PlayerSaveData : SaveDataBase
    {
        public float CurrentHealth;

        public Vector3 Position;

        public Quaternion Rotation;

        public WeaponSaveData EquippedWeapon;

        public List<ItemSaveData> itemList = new List<ItemSaveData>();
    }

}