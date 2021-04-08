using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.UI;
using Systems.Health;
using UI.Menus;
using UnityEngine.InputSystem;

namespace Character
{
    [RequireComponent(typeof(PlayerHealthComponent))]
    public class PlayerController : MonoBehaviour, IPausable
    {
        public CrossHairScript CrossHair => CrossHairComponent;
        [SerializeField]
        private CrossHairScript CrossHairComponent;

        public bool IsFiring;
        public bool IsReloading;
        public bool IsJumping;
        public bool IsRunning;

        public HealthComponent Health => healthComponent;
        private HealthComponent healthComponent;

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
    }

}