using SimpleRPG.Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using CharacterController = SimpleRPG.Core.CharacterController;

namespace SimpleRPG.Player
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class PlayerController : CharacterController
    {
        private Camera _mainCamera;

        private Player_IA _playerInputActions;
        private InputAction _mouseClickAction;
        private InputAction _mousePositionAction;
        

        protected override void Awake()
        {
            base.Awake();
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
                    if(target.IsDead)
                        continue;
                    StartFightAction(target);
                    return;
                }
            }
            
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
                StartMoveAction(hitInfo.point);
        }
        
    }
}