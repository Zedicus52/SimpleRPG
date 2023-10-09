using System;
using SimpleRPG.Combat;
using SimpleRPG.Core;
using SimpleRPG.DataPersistence;
using SimpleRPG.DataPersistence.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using CharacterController = SimpleRPG.Core.CharacterController;

namespace SimpleRPG.Player
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public sealed class PlayerController : CharacterController, IDataPersistence
    {
        private Camera _mainCamera;

        private Player_IA _playerInputActions;
        private InputAction _mouseClickAction;
        private InputAction _mousePositionAction;
        

        protected override void Awake()
        {
            base.Awake();
            GameContext.Instance.Saver.RegisterObject(this);
            _mainCamera = Camera.main;
            _playerInputActions = new Player_IA();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _mouseClickAction = _playerInputActions.Player.MouseClick;
            _mousePositionAction = _playerInputActions.Player.MousePosition;

            _mousePositionAction.Enable();
            _mouseClickAction.Enable();
            

            _mouseClickAction.performed += OnMouseClick;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            _mouseClickAction.Disable();
            _mousePositionAction.Disable();
        }

        private void OnDestroy()
        {
            GameContext.Instance.Saver.UnRegisterObject(this);
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

        public void LoadData(GameData gameData)
        {
            _transform.position = new Vector3(gameData.PlayerPosition.X, gameData.PlayerPosition.Y,
                gameData.PlayerPosition.Z);
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.PlayerPosition =
                new SerializableVector3(_transform.position.x, _transform.position.y, _transform.position.z);
        }
    }
}