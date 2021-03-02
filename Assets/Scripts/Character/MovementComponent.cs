using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

namespace Character
{
    public class MovementComponent : MonoBehaviour
    {
        //private PlayerInputAction PlayerActions;

        //private void Awake()
        //{
        //    PlayerActions = new PlayerInputAction();
        //}

        //private void Movement(InputAction.CallbackContext value)
        //{
        //    Debug.Log(value.ReadValue<Vector2>());
        //}


        //private void OnEnable()
        //{
        //    PlayerActions.Enable();
        //    PlayerActions.PlayerActionMap.Movement.performed += Movement;
        //}


        //private void OnDisable()
        //{
        //    PlayerActions.Disable();
        //    PlayerActions.PlayerActionMap.Movement.performed -= Movement;
        //}

        [SerializeField] private float WalkSpeed;
        [SerializeField] private float RunSpeed;
        [SerializeField] private float JumpForce;

        [SerializeField] private LayerMask JumpLayerMask;
        [SerializeField] private float JumpThreshold = 0.1f;
        [SerializeField] private float JumpLandingCheckRelay = 0.0f;

        // Components
        private PlayerController PlayerController;

        private Animator PlayerAnimator;

        private Rigidbody PlayerRigidbody;

        private NavMeshAgent PlayerNavmeshAgent;

        private Transform PlayerTransform;

        private Vector2 InputVector = Vector2.zero;

        private Vector3 MoveDirection = Vector3.zero;


        // Animation Hashes
        public readonly int MovementXHash = Animator.StringToHash("MovementX");
        public readonly int MovementYHash = Animator.StringToHash("MovementY");
        public readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
        public readonly int IsRunningHash = Animator.StringToHash("IsRunning");


        private void Awake()
        {
            PlayerTransform = transform;
            PlayerController = GetComponent<PlayerController>();
            PlayerAnimator = GetComponent<Animator>();
            PlayerRigidbody = GetComponent<Rigidbody>();
            PlayerNavmeshAgent = GetComponent<NavMeshAgent>();
        }

        public void OnMovement(InputValue value)
        {
            //Debug.Log(value.Get());
            InputVector = value.Get<Vector2>();

            PlayerAnimator.SetFloat(MovementXHash, InputVector.x);
            PlayerAnimator.SetFloat(MovementYHash, InputVector.y);
        }

        public void OnRun(InputValue value)
        {
            Debug.Log(value.isPressed);
            PlayerController.IsRunning = value.isPressed;
            PlayerAnimator.SetBool(IsRunningHash, value.isPressed);
        }

        public void OnJump(InputValue value)
        {
            if (PlayerController.IsJumping) return;

            PlayerNavmeshAgent.isStopped = true;
            PlayerNavmeshAgent.enabled = false;

            PlayerController.IsJumping = value.isPressed;
            PlayerAnimator.SetBool(IsJumpingHash, value.isPressed);
            PlayerRigidbody.AddForce((PlayerTransform.up + MoveDirection) * JumpForce, ForceMode.Impulse);

            InvokeRepeating(nameof(LandingCheck), JumpLandingCheckRelay, 0.1f);
        }

        private void LandingCheck()
        {
            if (!Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 100f, JumpLayerMask))
            {
                return;
            }

            Debug.Log(hit.distance);

            if (!(hit.distance < JumpThreshold) || !PlayerController.IsJumping)
            {
                return;
            }

            PlayerNavmeshAgent.enabled = true;
            PlayerNavmeshAgent.isStopped = false;

            PlayerController.IsJumping = false;
            PlayerAnimator.SetBool(IsJumpingHash, false);

            CancelInvoke(nameof(LandingCheck));
        }

        private void Update()
        {
            if (PlayerController.IsJumping)
                return;

            if (!(InputVector.magnitude > 0))
            {
                MoveDirection = Vector3.zero;
            }               
          
            MoveDirection = PlayerTransform.forward * InputVector.y + PlayerTransform.right * InputVector.x;

            float currentSpeed = PlayerController.IsRunning ? RunSpeed : WalkSpeed;

            Vector3 movementDirection = MoveDirection * (currentSpeed * Time.deltaTime);

            // PlayerTransform.position += movementDirection;

            PlayerNavmeshAgent.Move(movementDirection);


        }

        //private void OnCollisionEnter(Collision other)
        //{
        //    //if(!other.gameObject.CompareTag("Ground") && !PlayerController.IsJumping)
        //    //{
        //    //    return;
        //    //}

        //    //PlayerController.IsJumping = false;
        //    //PlayerAnimator.SetBool(IsJumpingHash, false);
        //}
    }
}

