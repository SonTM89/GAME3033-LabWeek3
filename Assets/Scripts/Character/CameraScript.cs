using Parent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class CameraScript : InputMonoBehaviour
    {
        [SerializeField]
        private float RotationPower = 10;

        [SerializeField]
        private float HorizontalDampling = 1;

        [SerializeField]
        private GameObject FollowTarget;

        private Transform FollowTargetTransform;
        private Vector2 PreviousMouseData = Vector2.zero;

        private new void Awake()
        {
            base.Awake();
            FollowTargetTransform = FollowTarget.transform;
        }

        private void OnLooked(InputAction.CallbackContext obj)
        {
            //Debug.Log("Camera Rotate");
            Vector2 aimValue = obj.ReadValue<Vector2>();

            Quaternion addedRoration = Quaternion.AngleAxis(Mathf.Lerp(PreviousMouseData.x, aimValue.x, 1f/ HorizontalDampling) * RotationPower, transform.up);

            FollowTargetTransform.rotation *= addedRoration;

            PreviousMouseData = aimValue;

            transform.rotation = Quaternion.Euler(0, FollowTargetTransform.rotation.eulerAngles.y, 0);

            FollowTargetTransform.localEulerAngles = Vector3.zero;
        }

        private void OnEnable()
        {
            base.OnEnable();
            GameInput.PlayerActionMap.Look.performed += OnLooked;
        }

        private void OnDisable()
        {
            base.OnDisable();
            GameInput.PlayerActionMap.Look.performed -= OnLooked;
        }
    }
}

