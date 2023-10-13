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

        [Header("Progression settings")] 
        [SerializeField] private int _skillsPointsForNewLevel = 3;
        [SerializeField] private int _experienceToNewLevel = 100;
        [SerializeField] private PlayerStatsPattern _playerStatsPattern;
        
        private Camera _mainCamera;

        private Player_IA _playerInputActions;
        private InputAction _mouseClickAction;
        private InputAction _mousePositionAction;

        private Checkpoint _lastCheckpoint;
        private WeaponHolder _lastWeapon;
        private NavMeshAgent _navMeshAgent;
        private PlayerStats _playerStats;

        private int _currentExperience;
        private int _currentLevel = 1;
        private int _availableSkillPoints;


        protected override void Awake()
        {
            base.Awake();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            GameContext.Instance.Saver.RegisterObject(this);
            _mainCamera = Camera.main;
            _playerInputActions = new Player_IA();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Health.CharacterDie += OnCharacterDie;
            
            _mouseClickAction = _playerInputActions.Player.MouseClick;
            _mousePositionAction = _playerInputActions.Player.MousePosition;

            _mousePositionAction.Enable();
            _mouseClickAction.Enable();
            

            _mouseClickAction.performed += OnMouseClick;
        }

        private void OnCharacterDie(int experience)
        {
            _currentExperience += experience;

            if (_currentExperience >= _currentLevel * _experienceToNewLevel)
            {
                _availableSkillPoints += _skillsPointsForNewLevel;
                _currentExperience -= _currentLevel * _experienceToNewLevel;
                ++_currentLevel;
            }
        }


        protected override void OnDisable()
        {
            base.OnDisable();

            Health.CharacterDie -= OnCharacterDie;
            
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
            _currentExperience = gameData.Player.CurrentExperience;
            _availableSkillPoints = gameData.Player.AvailableSkillPoints;
            _currentLevel = gameData.Player.CurrentLevel;
            var weapon = GameContext.Instance.GetWeaponById(gameData.Player.WeaponId);
            if (weapon)
                SetWeapon(weapon);
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
            gameData.Player.WeaponId = _currentWeapon.Id;
            gameData.Player.CurrentExperience = _currentExperience;
            gameData.Player.CurrentLevel = _currentLevel;
            gameData.Player.AvailableSkillPoints = _availableSkillPoints;
        }

        public void SetLastCheckpoint(Checkpoint checkpoint) => _lastCheckpoint = checkpoint;

        public void SetWeapon(WeaponSO weapon)
        {
            _currentWeapon = weapon;
            if(_lastWeapon)
                Destroy(_lastWeapon.gameObject);
            _lastWeapon = _currentWeapon.SpawnWeapon(_rightHandTransform, _leftHandTransform, _animator);
            _fighter ??= new Fighter(_navMeshAgent,
                _animator, _attackFrequency, _currentWeapon, _combatTarget, _rightHandTransform, _leftHandTransform);
            _fighter.SetWeapon(weapon);
        }
    }
}