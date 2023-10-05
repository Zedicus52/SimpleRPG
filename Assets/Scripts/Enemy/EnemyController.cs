using System;
using System.Collections;
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
        [SerializeField] private float _patrolTime;

        private Transform _playerTransform;
        private CombatTarget _playerCombatTarget;

        private Vector3 _startPosition;
        private float _patrolTimer;
        
    

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
                _patrolTimer = 0;
            }
            else if (distanceToPlayer > _chaseDistance 
                     && _returnToStartPosition && _patrolTimer >= _patrolTime)
            {
                StartMoveAction(_startPosition);
                _patrolTimer = 0;
            }

            _patrolTimer += Time.deltaTime;

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