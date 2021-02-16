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

        //Components
        private PlayerController PlayerController;
        private CrossHairScript PlayerCrossHair;
        private Animator PlayerAnimator;

        //Ref
        private Camera ViewCamera;

        private readonly int AimHorizontalHash = Animator.StringToHash("AimHorizontal");
        private readonly int AimVerticalHash = Animator.StringToHash("AimVertical");


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

            if(spawnWeapon)
            {
                WeaponComponent weapon = spawnWeapon.GetComponent<WeaponComponent>();
                if(weapon)
                {
                    GripIKLocation = weapon.GripLocation;
                }
            }
        }


        private void OnAnimatorIK(int layerIndex)
        {
            PlayerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            PlayerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, GripIKLocation.position);
        }

        private void OnReload(InputAction.CallbackContext obj)
        {
            Debug.Log("On Reload");
        }


        private void OnFire(InputAction.CallbackContext obj)
        {
            Debug.Log("On Fire");
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
            GameInput.PlayerActionMap.Reload.performed += OnReload;
        }

        private void OnDisable()
        {
            base.OnDisable();
            GameInput.PlayerActionMap.Look.performed -= OnLook;
            GameInput.PlayerActionMap.Fire.performed -= OnFire;
            GameInput.PlayerActionMap.Reload.performed -= OnReload;
        }
    }
}

