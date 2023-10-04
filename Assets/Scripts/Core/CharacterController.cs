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
        [SerializeField] private float _attackRange;
        [SerializeField] private float _damage;
        [SerializeField] private float _attackFrequency;
        
        private Animator _animator;
        protected Transform _transform;
        
        protected Mover _mover;
        protected Fighter _fighter;
        protected ActionScheduler _actionScheduler;
        
        private readonly int _characterSpeed = Animator.StringToHash("CharacterSpeed");

        
        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _transform = GetComponent<Transform>();
            
            _mover = new Mover(GetComponent<NavMeshAgent>());
            _fighter = new Fighter(GetComponent<NavMeshAgent>(), 
                _attackRange, _animator, _attackFrequency, _damage);
            _actionScheduler = new ActionScheduler();
        }
        
        protected virtual void Update()
        {
            UpdateAnimator();
            _fighter.Update();
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