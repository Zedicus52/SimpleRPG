using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace SimpleRPG.Player
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private Camera _mainCamera;
        private Animator _animator;
        private Transform _transform;

        private Player_IA _playerInputActions;
        private InputAction _mouseClickAction;
        private InputAction _mousePositionAction;
        private readonly int _playerSpeed = Animator.StringToHash("PlayerSpeed");

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _transform = GetComponent<Transform>();

            _mainCamera = Camera.main;
            _playerInputActions = new Player_IA();
        }

        private void OnEnable()
        {
            _mouseClickAction = _playerInputActions.Player.MouseClick;
            _mousePositionAction = _playerInputActions.Player.MousePosition;

            _mousePositionAction.Enable();
            _mouseClickAction.Enable();

            _mouseClickAction.performed += OnMouseClick;
        }

        private void Update()
        {
            if(_mouseClickAction.IsPressed())
                OnMouseClick();
            
            UpdateAnimator();
        }

        private void OnDisable()
        {
            _mouseClickAction.Disable();
            _mousePositionAction.Disable();
        }

        private void OnMouseClick(InputAction.CallbackContext context) => OnMouseClick();

        private void OnMouseClick()
        {
            Vector2 mousePos = _mousePositionAction.ReadValue<Vector2>();
            Ray ray = _mainCamera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                _navMeshAgent.SetDestination(hitInfo.point);
            }
        }

        private void UpdateAnimator()
        {
            Vector3 localVelocity = _transform.InverseTransformDirection(_navMeshAgent.velocity);
            _animator.SetFloat(_playerSpeed, localVelocity.z);
        }
    }
}