using System;
using SimpleRPG.Combat;
using SimpleRPG.Player;
using UnityEngine;
using CharacterController = SimpleRPG.Core.CharacterController;

namespace SimpleRPG.Enemy
{
    public class EnemyController : CharacterController
    {
        [Header("Enemy chase settings")]
        [SerializeField] private float _chaseDistance;
        [SerializeField] private bool _returnToStartPosition;

        private Transform _playerTransform;
        private CombatTarget _playerCombatTarget;

        private Vector3 _startPosition;
        
    

        protected override void Awake()
        {
            base.Awake();
            _startPosition = _transform.position;
            _playerTransform = FindObjectOfType<PlayerController>().transform;
            _playerCombatTarget = FindObjectOfType<PlayerController>().GetComponent<CombatTarget>();
        }

        protected override void Update()
        {
            base.Update();

            float distanceToPlayer = GetDistanceToPlayer();
            if (distanceToPlayer < _chaseDistance)
            {
                StartFightAction(_playerCombatTarget);
            }
            else if (distanceToPlayer > _chaseDistance && _returnToStartPosition)
            {
                StartMoveAction(_startPosition);
            }
            
        }

        private float GetDistanceToPlayer()
        {
            return Vector3.Distance(_transform.position, _playerTransform.position);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
            Gizmos.color = Color.white;
        }
    }
}