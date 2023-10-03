using UnityEngine;
using UnityEngine.InputSystem;

namespace SimpleRPG.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Player_IA _playerInputActions;
        private InputAction _mouseClickAction;
        private InputAction _mousePositionAction;
        
        private void OnEnable()
        {
            _mouseClickAction = _playerInputActions.Player.MouseClick;
            _mousePositionAction = _playerInputActions.Player.MousePosition;

            _mousePositionAction.Enable();
            _mouseClickAction.Enable();

            //_mouseClickAction.performed += OnMouseClick;
        }

        private void OnDisable()
        {
            _mouseClickAction.Disable();
            _mousePositionAction.Disable();
        }
    }
}