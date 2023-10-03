using SimpleRPG.Abstraction;
using UnityEngine;
using UnityEngine.AI;

namespace SimpleRPG.Combat
{
    public class Fighter : IAction
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly float _attackDistance;
        private readonly Transform _transform;
        private CombatTarget _currentTarget;
        
        public Fighter(NavMeshAgent agent, float attackDistance)
        {
            _navMeshAgent = agent;
            _attackDistance = attackDistance;
        }
        public void Attack(CombatTarget target)
        {
            _currentTarget = target;

            if (Vector3.Distance(target.transform.position, _navMeshAgent.transform.position) <= _attackDistance)
            {
                Debug.Log($"Attack {target}");
                return;
            }

            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(target.transform.position);

        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
            _currentTarget = null;
        }

        public void Update()
        {
            if (!_currentTarget)
                return;


            if (Vector3.Distance(_currentTarget.transform.position, _navMeshAgent.transform.position) <=
                _attackDistance)
                _navMeshAgent.isStopped = true;
        }
    }
}   