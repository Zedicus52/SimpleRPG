using System;
using SimpleRPG.Combat;
using SimpleRPG.Core;
using SimpleRPG.DataPersistence;
using SimpleRPG.DataPersistence.Data;
using SimpleRPG.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

        private Checkpoint _lastCheckpoint;
        

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
            var pos = gameData.Player.Position;
            var rotation = gameData.Player.Rotation;
            _combatTarget.CreateHealth(gameData.Player.Health);
            _transform.position = new Vector3(pos.X, pos.Y, pos.Z);
            _transform.rotation = Quaternion.Euler(rotation.X, rotation.Y, rotation.Z);
        }

        public void SaveData(ref GameData gameData)
        {
            var rotation = _transform.rotation;
            var position = _lastCheckpoint == null ? 
                LevelContext.Instance.SpawnPoint.position : _lastCheckpoint.SpawnPoint.position;

            gameData.Player.LastSceneId = SceneManager.GetActiveScene().buildIndex;
            gameData.Player.Health = _combatTarget.CharacterHealth.CurrentHealth;
            gameData.Player.Position = new SerializableVector3(position.x, position.y, position.z);
            gameData.Player.Rotation = new SerializableVector3(rotation.x, rotation.y, rotation.z);
        }

        public void SetLastCheckpoint(Checkpoint checkpoint) => _lastCheckpoint = checkpoint;

        public void SetWeapon(WeaponSO weapon)
        {
            _currentWeapon = weapon;
            _currentWeapon.SpawnWeapon(_rightHandTransform, _leftHandTransform, _animator);
            _fighter.SetDamage(_currentWeapon.Damage);
            _fighter.SetAttackDistance(_currentWeapon.AttackRange);
        }
    }
}