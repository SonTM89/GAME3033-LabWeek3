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
        [Header("Weapon To SPawn"), SerializeField]
        //private GameObject WeaponToSpawn;
        private WeaponScriptable WeaponToSpawn;

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

        private WeaponComponent EquippedWeapon;

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
            Debug.Log("On Fire");

            //FiringPressed = pressed.ReadValue<float>() == 1 ? true : false;
            FiringPressed = pressed.isPressed;

            if (EquippedWeapon == null) return;

            if(FiringPressed)
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
            if (EquippedWeapon == null) return;

            if (EquippedWeapon.WeaponInformation.BulletsAvailable <= 0 && EquippedWeapon.WeaponInformation.BulletsInClip <= 0)
            {
                return;
            }

            PlayerController.IsFiring = true;

            PlayerAnimator.SetBool(IsFiringHash, true);

            EquippedWeapon.StartFiringWeapon();

        }

        public void StopFiring()
        {
            if (EquippedWeapon == null) return;

            PlayerController.IsFiring = false;

            PlayerAnimator.SetBool(IsFiringHash, false);

            EquippedWeapon.StopFiringWeapon();
        }


        private void OnReload(InputValue button)
        {
            Debug.Log("On Reload");

            if (EquippedWeapon == null) return;

            //bool isReloading = button.isPressed;
            StartReloading();
            
        }

        public void StartReloading()
        {
            if (EquippedWeapon == null) return;

            if (EquippedWeapon.WeaponInformation.BulletsAvailable <= 0 && PlayerController.IsFiring)
            {
                StopFiring();
                return;
            }


            PlayerController.IsReloading = true;
            PlayerAnimator.SetBool(IsReloadingHash, true);
            EquippedWeapon.StartReloading();

            InvokeRepeating(nameof(StopReloading), 0, 0.1f);
        }

        private void StopReloading()
        {
            if (EquippedWeapon == null) return;

            if (PlayerAnimator.GetBool(IsReloadingHash)) return;

            PlayerController.IsReloading = false;
            EquippedWeapon.StopReloading();
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
            Destroy(EquippedWeapon.gameObject);
            EquippedWeapon = null;
        }

        public void EquipWeapon(WeaponScriptable weaponScriptable)
        {
            if (weaponScriptable == null) return;

            GameObject spawnWeapon = Instantiate(weaponScriptable.ItemPrefab, WeaponSocketLocation.position, WeaponSocketLocation.rotation, WeaponSocketLocation);

            if (!spawnWeapon) return;

            EquippedWeapon = spawnWeapon.GetComponent<WeaponComponent>();

            if (!EquippedWeapon) return;

            EquippedWeapon.Initialize(this, weaponScriptable);

            PlayerEvents.Invoke_OnWeaponEquipped(EquippedWeapon);

            GripIKLocation = EquippedWeapon.GripLocation;
            PlayerAnimator.SetInteger(WeaponTypeHash, (int)EquippedWeapon.WeaponInformation.WeaponType);
        }
    }
}

