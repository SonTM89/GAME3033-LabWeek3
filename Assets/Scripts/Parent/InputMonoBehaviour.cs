using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Parent
{
    public class InputMonoBehaviour : MonoBehaviour
    {

        protected PlayerInputAction GameInput;

        protected void Awake()
        {
            GameInput = new PlayerInputAction();
        }


        protected void OnEnable()
        {
            GameInput.Enable();
        }


        protected void OnDisable()
        {
            GameInput.Disable();
        }
    }
}

