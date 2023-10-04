using System;
using SimpleRPG.Abstraction;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace SimpleRPG.Player
{
    public class Mover : IAction
    {
        public Vector3 Velocity => _navMeshAgent.velocity;
        
        private readonly NavMeshAgent _navMeshAgent;
        public Mover(NavMeshAgent agent)
        {
            _navMeshAgent = agent;
        }

        public void StartAction(Vector3 destinationPoint)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(destinationPoint);

        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }
    }
}