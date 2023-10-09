using SimpleRPG.Abstraction;
using SimpleRPG.Combat;
using UnityEngine;
using UnityEngine.AI;


namespace SimpleRPG.Player
{
    public class Mover : IAction
    {
        public Vector3 Velocity => _navMeshAgent.velocity;
        
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Health _health;
        public Mover(NavMeshAgent agent, Health health)
        {
            _health = health;
            _navMeshAgent = agent;
        }

        public void StartAction(Vector3 destinationPoint)
        {
            if(_health.IsDead)
                return;
            
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(destinationPoint);

        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }
    }
}