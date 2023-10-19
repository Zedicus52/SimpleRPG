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
        private readonly float _maxPathLength;
        private readonly Health _health;
        public Mover(NavMeshAgent agent, Health health, float maxPathLength)
        {
            _maxPathLength = maxPathLength;
            _health = health;
            _navMeshAgent = agent;
        }

        public void StartAction(Vector3 destinationPoint)
        {
            if(_health.IsDead)
                return;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(_navMeshAgent.transform.position, 
                destinationPoint, NavMesh.AllAreas, path);
            if(hasPath == false) return;
            if(path.status != NavMeshPathStatus.PathComplete) return;
            if(GetPathLength(path) > _maxPathLength) return;
            
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(destinationPoint);

        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length-1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }
    }
}