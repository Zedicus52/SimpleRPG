using System;
using SimpleRPG.Combat;
using SimpleRPG.Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace SimpleRPG.Player
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Fighter Settings")]
        [SerializeField] private float _attackRange;
        [SerializeField] private float _attackFrequency;
        
        private Camera _mainCamera;
        private Animator _animator;
        private Transform _transform;
        
        private Player_IA _playerInputActions;
        private InputAction _mouseClickAction;
        private InputAction _mousePositionAction;

        private Mover _mover;
        private Fighter _fighter;
        private ActionScheduler _actionScheduler;
        
        private readonly int _playerSpeed = Animator.StringToHash("PlayerSpeed");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _transform = GetComponent<Transform>();
            _mainCamera = Camera.main;
            
            _mover = new Mover(GetComponent<NavMeshAgent>());
            _fighter = new Fighter(GetComponent<NavMeshAgent>(), _attackRange, _animator, _attackFrequency);
            _actionScheduler = new ActionScheduler();
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
            UpdateAnimator();
            _fighter.Update();

        }

        //Animation event
        public void Hit()
        {
            _fighter.Attack();
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

            RaycastHit[] results = Physics.RaycastAll(ray);
            
            foreach (RaycastHit result in results)
            {
                if (result.transform.TryGetComponent(out CombatTarget target))
                {
                    _actionScheduler.StartNewAction(_fighter);
                    //_animator.SetTrigger(_attack);
                    _fighter.SetTarget(target);
                    return;
                }
            }
            
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                _actionScheduler.StartNewAction(_mover);
                _mover.StartAction(hitInfo.point);
            }
        }
        private void UpdateAnimator()
        {
            Vector3 localVelocity = _transform.InverseTransformDirection(_mover.Velocity);
            _animator.SetFloat(_playerSpeed, localVelocity.z);
        }
    }
}