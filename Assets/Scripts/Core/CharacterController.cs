using System;
using SimpleRPG.Cinematic;
using SimpleRPG.Combat;
using SimpleRPG.Player;
using UnityEngine;
using UnityEngine.AI;

namespace SimpleRPG.Core
{
    [RequireComponent(
         typeof(NavMeshAgent), 
        typeof(Animator), 
        typeof(CombatTarget))]
    public abstract class CharacterController : MonoBehaviour
    {
        [Header("Fighter Settings")]
        [SerializeField] protected float _attackRange;
        [SerializeField] private float _damage;
        [SerializeField] private float _attackFrequency;
        
        private Animator _animator;
        protected Transform _transform;
        private CombatTarget _combatTarget;


        private Mover _mover;
        private Fighter _fighter;
        private ActionScheduler _actionScheduler;
        
        private readonly int _characterSpeed = Animator.StringToHash("CharacterSpeed");

        
        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _transform = GetComponent<Transform>();
            _combatTarget = GetComponent<CombatTarget>();
        }

        protected virtual void Start()
        {
            _mover = new Mover(GetComponent<NavMeshAgent>(), GetComponent<CombatTarget>().CharacterHealth);
            _fighter = new Fighter(GetComponent<NavMeshAgent>(), 
                _attackRange, _animator, _attackFrequency, _damage, _combatTarget);
            _actionScheduler = new ActionScheduler();
        }

        protected virtual void OnEnable()
        {
            CinematicTriggerZone.CinematicStarted += OnCinematicStared;
            CinematicTriggerZone.CinematicEnded += OnCinematicEnded;
        }

        protected virtual void OnDisable()
        {
            CinematicTriggerZone.CinematicStarted -= OnCinematicStared;
            CinematicTriggerZone.CinematicEnded -= OnCinematicEnded;
        }

        protected virtual void Update()
        {
            if(_combatTarget.IsDead)
                return;
            
            UpdateAnimator();
            _fighter.Update();
        }

        private void OnCinematicStared()
        {
            _actionScheduler.CancelCurrentAction();
            _actionScheduler.SetPossibilityToStartNewAction(false);
        }

        private void OnCinematicEnded()
        {
            _actionScheduler.SetPossibilityToStartNewAction(true);
        }

        
        public void Hit() => _fighter.Attack();

        private void UpdateAnimator()
        {
            Vector3 localVelocity = _transform.InverseTransformDirection(_mover.Velocity);
            _animator.SetFloat(_characterSpeed, localVelocity.z);
        }

        protected void StartMoveAction(Vector3 destinationPoint)
        {
            _actionScheduler.StartNewAction(_mover);
            _mover.StartAction(destinationPoint);
        }

        protected void StartFightAction(CombatTarget target)
        {
            _actionScheduler.StartNewAction(_fighter);
            _fighter.SetTarget(target);
        }
    }
}