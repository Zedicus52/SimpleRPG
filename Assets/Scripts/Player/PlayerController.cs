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

        private InputAction _mouseClickAction;
        private InputAction _mousePositionAction;
        private InputAction _skillsOpenedAction;

        private Checkpoint _lastCheckpoint;
        private WeaponHolder _lastWeapon;
        private NavMeshAgent _navMeshAgent;
        private PlayerStats _playerStats;
        

        protected override void Awake()
        {
            base.Awake();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            GameContext.Instance.Saver.RegisterObject(this);
            _mainCamera = Camera.main;
        }

        protected override void Start()
        {
            base.Start();
            _playerStats ??= new PlayerStats(_experienceToNewLevel,
                _skillsPointsForNewLevel, _playerStatsPattern);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Health.CharacterDie += OnCharacterDie;

            Player_IA input = LevelContext.Instance.PlayerInput;
            _mouseClickAction = input.Player.MouseClick;
            _mousePositionAction = input.Player.MousePosition;
            _skillsOpenedAction = input.PlayerUI.OpenSkillsTree;

            _mousePositionAction.Enable();
            _mouseClickAction.Enable();
            _skillsOpenedAction.Enable();

            _skillsOpenedAction.performed += OnSkillsOpened;
            _mouseClickAction.performed += OnMouseClick;
        }

        private void OnSkillsOpened(InputAction.CallbackContext obj)
        {
            if (LevelContext.Instance.StatsUI.IsOpened)
            {
                Time.timeScale = 0f;
                LevelContext.Instance.StatsUI.Initialize(ref _playerStats);
            }
            else
            {
                UpdateStats();
                Time.timeScale = 1f;
            }
        }

        private void UpdateStats()
        {
            Debug.Log($"Update stats {_playerStats.MaxHealth}");
            _combatTarget.CharacterHealth.SetMaxHealth(_playerStats.MaxHealth);
            _navMeshAgent.speed = _playerStats.MaxMovementSpeed;
            _fighter.SetStrengthMultiplier(_playerStats.StrengthMultiplier);
            _fighter.SetAttackFrequency(_playerStats.AgilityMultiplier);
        }
        
        private void OnCharacterDie(int experience)
        { 
            _playerStats.AddExperience(experience);
        }


        protected override void OnDisable()
        {
            base.OnDisable();

            Health.CharacterDie -= OnCharacterDie;
            
            _mouseClickAction.Disable();
            _mousePositionAction.Disable();
            _skillsOpenedAction.Disable();
            
            _mouseClickAction.performed -= OnMouseClick;
            _skillsOpenedAction.performed -= OnSkillsOpened;

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
            _transform.position = new Vector3(pos.X, pos.Y, pos.Z);
            _transform.rotation = Quaternion.Euler(rotation.X, rotation.Y, rotation.Z);
            
            var stats = gameData.Player.Stats;
            _playerStats = new PlayerStats(stats.MaxHealth, stats.StrengthMultiplier, stats.MovementSpeed,
                stats.AgilityMultiplier, _playerStatsPattern, _experienceToNewLevel, _skillsPointsForNewLevel);
            _combatTarget.CreateHealth(_playerStats.MaxHealth, gameData.Player.CurrentHealth);
            
            _playerStats.SetExperience(gameData.Player.CurrentExperience);
            _playerStats.SetCurrentLevel(gameData.Player.CurrentLevel);
            _playerStats.SetAvailableSkillPoints(gameData.Player.AvailableSkillPoints);
            
            var weapon = GameContext.Instance.GetWeaponById(gameData.Player.WeaponId);
            if (weapon)
                SetWeapon(weapon);
            
            UpdateStats();

        }

        public void SaveData(ref GameData gameData)
        {
            var rotation = _transform.rotation;
            var position = _lastCheckpoint == null ? 
                LevelContext.Instance.SpawnPoint.position : _lastCheckpoint.SpawnPoint.position;

            gameData.Player.LastSceneId = SceneManager.GetActiveScene().buildIndex;
            gameData.Player.CurrentHealth = _combatTarget.CharacterHealth.CurrentHealth;
            gameData.Player.Position = new SerializableVector3(position.x, position.y, position.z);
            gameData.Player.Rotation = new SerializableVector3(rotation.x, rotation.y, rotation.z);
            gameData.Player.WeaponId = _currentWeapon.Id;
            gameData.Player.CurrentExperience = _playerStats.CurrentExperience;
            gameData.Player.CurrentLevel = _playerStats.CurrentLevel;
            gameData.Player.AvailableSkillPoints = _playerStats.AvailableSkillPoints;

            Debug.Log($"Save data: {_playerStats.MaxHealth}");
            
            gameData.Player.Stats.MaxHealth = _playerStats.MaxHealth;
            gameData.Player.Stats.AgilityMultiplier = _playerStats.AgilityMultiplier;
            gameData.Player.Stats.StrengthMultiplier = _playerStats.StrengthMultiplier;
            gameData.Player.Stats.MovementSpeed = _playerStats.MaxMovementSpeed;
        }

        public void SetLastCheckpoint(Checkpoint checkpoint) => _lastCheckpoint = checkpoint;

        public void SetWeapon(WeaponSO weapon)
        {
            _currentWeapon = weapon;
            if(_lastWeapon)
                Destroy(_lastWeapon.gameObject);
            _lastWeapon = _currentWeapon.SpawnWeapon(_rightHandTransform, _leftHandTransform, _animator);
            _fighter ??= new Fighter(_navMeshAgent,
                _animator, _attackFrequency, _currentWeapon, _combatTarget, 
                _rightHandTransform, _leftHandTransform, _playerStats.StrengthMultiplier);
            _fighter.SetWeapon(weapon);
        }
    }
}