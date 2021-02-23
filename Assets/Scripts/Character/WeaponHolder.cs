using Character.UI;
using Parent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class WeaponHolder : InputMonoBehaviour
    {
        [Header("Weapon To SPawn"), SerializeField]
        private GameObject WeaponToSpawn;

        [SerializeField]
        private Transform WeaponSocketLocation;

        private Transform GripIKLocation;

        private bool WasFiring = false;

        private bool FiringPressed;

        //Components
        public PlayerController Controller => PlayerController;
        private PlayerController PlayerController;
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
            base.Awake();

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
            GameObject spawnWeapon = Instantiate(WeaponToSpawn, WeaponSocketLocation.position, WeaponSocketLocation.rotation, WeaponSocketLocation);

            if (!spawnWeapon) return;
            
            EquippedWeapon = spawnWeapon.GetComponent<WeaponComponent>();
            
            if (!EquippedWeapon) return;

            EquippedWeapon.Initialize(this, PlayerCrossHair);
            
            GripIKLocation = EquippedWeapon.GripLocation;
            PlayerAnimator.SetInteger(WeaponTypeHash, (int)EquippedWeapon.WeaponInformation.WeaponType);
                      
        }


        private void OnAnimatorIK(int layerIndex)
        {
            PlayerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            PlayerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, GripIKLocation.position);
        }


        private void OnFire(InputAction.CallbackContext pressed)
        {
            Debug.Log("On Fire");

            FiringPressed = pressed.ReadValue<float>() == 1 ? true : false;

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
            PlayerController.IsFiring = false;

            PlayerAnimator.SetBool(IsFiringHash, false);

            EquippedWeapon.StopFiringWeapon();
        }


        private void OnReload(InputValue button)
        {
            Debug.Log("On Reload");

            //bool isReloading = button.isPressed;
            StartReloading();
            
        }

        public void StartReloading()
        {
            if (EquippedWeapon.WeaponInformation.BulletsAvailable <= 0)
            {
                if (PlayerController.IsFiring)
                    StopFiring();
                return;
            }

            if (PlayerController.IsFiring)
            {
                WasFiring = true;
                StopFiring();
            }

            PlayerController.IsReloading = true;
            PlayerAnimator.SetBool(IsReloadingHash, true);
            EquippedWeapon.StartReloading();

            InvokeRepeating(nameof(StopReloading), 0, 0.1f);
        }

        private void StopReloading()
        {
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


        public void OnLook(InputAction.CallbackContext obj)
        {
            Vector2 independentMousePosition = ViewCamera.ScreenToViewportPoint(PlayerCrossHair.CurrentAimPosition);

            Debug.Log(independentMousePosition);

            PlayerAnimator.SetFloat(AimHorizontalHash, independentMousePosition.x);
            PlayerAnimator.SetFloat(AimVerticalHash, independentMousePosition.y);
        }


        private void OnEnable()
        {
            base.OnEnable();
            GameInput.PlayerActionMap.Look.performed += OnLook;
            GameInput.PlayerActionMap.Fire.performed += OnFire;
            //GameInput.PlayerActionMap.Reload.performed += OnReload;
        }

        private void OnDisable()
        {
            base.OnDisable();
            GameInput.PlayerActionMap.Look.performed -= OnLook;
            GameInput.PlayerActionMap.Fire.performed -= OnFire;
            //GameInput.PlayerActionMap.Reload.performed -= OnReload;
        }
    }
}

