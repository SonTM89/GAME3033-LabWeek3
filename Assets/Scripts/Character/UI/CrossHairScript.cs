using Parent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.UI
{
    public class CrossHairScript : InputMonoBehaviour, IPausable
    {
        public Vector2 MouseSensitivity;

        public bool Inverted = false;

        public Vector2 CurrentAimPosition { get; private set; }

        [SerializeField, Range(0, 1)]
        private float CrosshairHorizontalPercentage = 0.25f;

        private float HorizontalOffset;
        private float MaxHorizontalDeltaConstrain;
        private float MinHorizontalDeltaConstrain;

        [SerializeField, Range(0, 1)]
        private float CrosshairVerticalPercentage = 0.25f;

        private float VerticalOffset;
        private float MaxVerticalDeltaConstrain;
        private float MinVerticalDeltaConstrain;

        private Vector2 CrosshairStartingPosition;
        private Vector2 CurrentLookDeltas;

        private bool GamePaused = false;


        // Start is called before the first frame update
        void Start()
        {
            if (GameManager.Instance.CursorActive)
            {
                AppEvents.Invoke_OnMouseCursorEnable(false);
            }

            CrosshairStartingPosition = new Vector2(Screen.width / 2f, Screen.height / 2f) ;

            HorizontalOffset = (Screen.width * CrosshairHorizontalPercentage) / 2f;
            MinHorizontalDeltaConstrain = -(Screen.width / 2f) + HorizontalOffset;
            MaxHorizontalDeltaConstrain = (Screen.width / 2f) - HorizontalOffset;

            VerticalOffset = (Screen.height * CrosshairVerticalPercentage) / 2f;
            MinVerticalDeltaConstrain = -(Screen.height / 2f) + VerticalOffset;
            MaxVerticalDeltaConstrain = (Screen.height / 2f) - VerticalOffset;


        }


        private void OnLook(InputAction.CallbackContext delta)
        {
            //if (GamePaused) return;

            //Debug.Log(delta.ReadValue<Vector2>());
            Vector2 mouseDelta = delta.ReadValue<Vector2>();

            CurrentLookDeltas.x += mouseDelta.x * MouseSensitivity.x;

            if(CurrentLookDeltas.x >= MaxHorizontalDeltaConstrain || CurrentLookDeltas.x <= MinHorizontalDeltaConstrain)
            {
                CurrentLookDeltas.x -= mouseDelta.x * MouseSensitivity.x;
            }

            CurrentLookDeltas.y += mouseDelta.y * MouseSensitivity.y;

            if (CurrentLookDeltas.y >= MaxVerticalDeltaConstrain || CurrentLookDeltas.y <= MinVerticalDeltaConstrain)
            {
                CurrentLookDeltas.y -= mouseDelta.y * MouseSensitivity.y;
            }

        }


        private void Update()
        {
            //if (GamePaused) return;

            float croshairXPosition = CrosshairStartingPosition.x + CurrentLookDeltas.x;
            float crosshairYPosition = Inverted ? CrosshairStartingPosition.y - CurrentLookDeltas.y : CrosshairStartingPosition.y + CurrentLookDeltas.y;

            CurrentAimPosition = new Vector2(croshairXPosition, crosshairYPosition);

            transform.position = CurrentAimPosition;
        }


        private new void OnEnable()
        {
            base.OnEnable();
            GameInput.PlayerActionMap.Look.performed += OnLook;
        }

        private new void OnDisable()
        {
            base.OnDisable();
            GameInput.PlayerActionMap.Look.performed -= OnLook;
        }

        public void PauseMenu()
        {
            GamePaused = true;
        }

        public void UnPauseMenu()
        {
            GamePaused = false;
        }
    }
}

