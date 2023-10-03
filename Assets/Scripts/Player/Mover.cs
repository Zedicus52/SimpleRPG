using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace SimpleRPG.Player
{
    public class Mover
    {
        public Vector3 Velocity => _navMeshAgent.velocity;
        
        private readonly NavMeshAgent _navMeshAgent;
        public Mover(NavMeshAgent agent)
        {
            _navMeshAgent = agent;
        }
        
        public void SetDestinationPoint(Vector3 destinationPoint) => 
            _navMeshAgent.SetDestination(destinationPoint);
    }
}