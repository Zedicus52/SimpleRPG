using SimpleRPG.Abstraction;
using UnityEngine;
using UnityEngine.AI;

namespace SimpleRPG.Combat
{
    public class Fighter : IAction
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly float _attackDistance;
        private readonly float _attackFrequency;
        private readonly Animator _animator;
        
        private readonly int _attack = Animator.StringToHash("Attack");

        private CombatTarget _currentTarget;
        private float _lastAttackTime;
        
        public Fighter(NavMeshAgent agent, float attackDistance, 
            Animator animator, float attackFrequency)
        {
            _navMeshAgent = agent;
            _attackDistance = attackDistance;
            _attackFrequency = attackFrequency;
            _animator = animator;
        }
        public void Attack()
        {
            if (_currentTarget == null)
                return;
            
            if (Vector3.Distance(_currentTarget.transform.position, _navMeshAgent.transform.position) <= _attackDistance)
            {
                Debug.Log($"Attack {_currentTarget}");
            }
            
        }

        public void SetTarget(CombatTarget target)
        {
            if (target != null && _currentTarget != target)
            {
                _currentTarget = target;
                _lastAttackTime = 0;
                MoveToTarget();
            }
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
            _currentTarget = null;
        }

        public void Update()
        {
            _lastAttackTime += Time.deltaTime; 
            
            if (!_currentTarget)
                return;


            if (Vector3.Distance(_currentTarget.transform.position, _navMeshAgent.transform.position) <=
                _attackDistance && _lastAttackTime >= _attackFrequency)
            {
                _navMeshAgent.isStopped = true;
                _animator.SetTrigger(_attack);
                _lastAttackTime = 0;
            }
        }

        private void MoveToTarget()
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_currentTarget.transform.position);
        }
    }
}   