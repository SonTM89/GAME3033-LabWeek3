using Character.UI;
using Parent;
using Scriptable_Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class WeaponHolder : MonoBehaviour
    {
        [Header("Weapon To SPawn")]
        //private GameObject WeaponToSpawn;
        [SerializeField] private WeaponScriptable WeaponToSpawn;

        [SerializeField]
        private Transform WeaponSocketLocation;

        private Transform GripIKLocation;

        private bool WasFiring = false;

        private bool FiringPressed;

        //Components
        public PlayerController Controller => PlayerController;
        private PlayerController PlayerController;

        public CrossHairScript CrossHair => PlayerCrossHair;
        private CrossHairScript PlayerCrossHair;

        private Animator PlayerAnimator;

        //Ref
        private Camera ViewCamera;

        public WeaponComponent EquippedWeapon => WeaponComponent;
        private WeaponComponent WeaponComponent;

        private readonly int AimHorizontalHash = Animator.StringToHash("AimHorizontal");
        private readonly int AimVerticalHash = Animator.StringToHash("AimVertical");
        private readonly int IsFiringHash = Animator.StringToHash("IsFiring");
        private readonly int IsReloadingHash = Animator.StringToHash("IsReloading");
        private readonly int WeaponTypeHash = Animator.StringToHash("WeaponType");


        private void Awake()
        {

            PlayerAnimator = GetComponent<Animator>();
            PlayerController = GetComponent<PlayerController>();
            if(PlayerController)
            {
                PlayerCrossHair = PlayerController.CrossHair;
            }

            ViewCamera = Camera.main;
        }



        // Start is called before the first frame update
        void Start()
        {

            if(WeaponToSpawn) EquipWeapon(WeaponToSpawn);    
        }


        private void OnAnimatorIK(int layerIndex)
        {
            if (GripIKLocation == null) return;

            PlayerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            PlayerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, GripIKLocation.position);
        }


        private void OnFire(InputValue pressed)
        {
            //FiringPressed = pressed.ReadValue<float>() == 1 ? true : false;
            FiringPressed = pressed.isPressed;

            if (WeaponComponent == null) return;

            Debug.Log("On Fire");

            if (FiringPressed)
            {
                StartFiring();
            }
            else
            {
                StopFiring();
            }
        
        }

        public void StartFiring()
        {
            if (WeaponComponent == null) return;

            if (WeaponComponent.WeaponInformation.BulletsAvailable <= 0 && WeaponComponent.WeaponInformation.BulletsInClip <= 0)
            {
                return;
            }

            PlayerController.IsFiring = true;

            PlayerAnimator.SetBool(IsFiringHash, true);

            WeaponComponent.StartFiringWeapon();

        }

        public void StopFiring()
        {
            if (WeaponComponent == null) return;

            PlayerController.IsFiring = false;

            PlayerAnimator.SetBool(IsFiringHash, false);

            WeaponComponent.StopFiringWeapon();
        }


        private void OnReload(InputValue button)
        {
            if (WeaponComponent == null) return;

            Debug.Log("On Reload");

            //bool isReloading = button.isPressed;
            StartReloading();
            
        }

        public void StartReloading()
        {
            if (WeaponComponent == null) return;

            if (WeaponComponent.WeaponInformation.BulletsAvailable <= 0 && PlayerController.IsFiring)
            {
                StopFiring();
                return;
            }


            PlayerController.IsReloading = true;
            PlayerAnimator.SetBool(IsReloadingHash, true);
            WeaponComponent.StartReloading();

            InvokeRepeating(nameof(StopReloading), 0, 0.1f);
        }

        private void StopReloading()
        {
            if (WeaponComponent == null) return;

            if (PlayerAnimator.GetBool(IsReloadingHash)) return;

            PlayerController.IsReloading = false;
            WeaponComponent.StopReloading();
            CancelInvoke(nameof(StopReloading));

            if (!WasFiring || !FiringPressed)
            {
                return;
            }

            StartFiring();
            WasFiring = false;
        }


        public void OnLook(InputValue obj)
        {
            Vector2 independentMousePosition = ViewCamera.ScreenToViewportPoint(PlayerCrossHair.CurrentAimPosition);

            //Debug.Log(independentMousePosition);

            PlayerAnimator.SetFloat(AimHorizontalHash, independentMousePosition.x);
            PlayerAnimator.SetFloat(AimVerticalHash, independentMousePosition.y);
        }


        public void UnEquipItem()
        {
            Destroy(WeaponComponent.gameObject);
            WeaponComponent = null;
        }

        public void EquipWeapon(WeaponScriptable weaponScriptable)
        {
            if (weaponScriptable == null) return;

            GameObject spawnWeapon = Instantiate(weaponScriptable.ItemPrefab, WeaponSocketLocation.position, WeaponSocketLocation.rotation, WeaponSocketLocation);

            if (!spawnWeapon) return;

            WeaponComponent = spawnWeapon.GetComponent<WeaponComponent>();

            if (!WeaponComponent) return;

            WeaponComponent.Initialize(this, weaponScriptable);

            PlayerEvents.Invoke_OnWeaponEquipped(WeaponComponent);

            GripIKLocation = WeaponComponent.GripLocation;
            PlayerAnimator.SetInteger(WeaponTypeHash, (int)WeaponComponent.WeaponInformation.WeaponType);
        }
    }
}

