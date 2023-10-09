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
        [SerializeField] private float _patrolPointTolerance;
        [SerializeField] private PatrolPath _patrolPath; 

        private Transform _playerTransform;
        private CombatTarget _playerCombatTarget;

        private Vector3 _startPosition;
        private float _patrolTimer;
        private int _currentPatrolPointIndex;
        private PatrolPoint _nextPatrolPoint;

        private bool _isAttack;
    

        protected override void Awake()
        {
            base.Awake();
            if (_patrolPath)
            {
                _transform.position = _patrolPath.GetFirstPatrolPoint().Position;
                _nextPatrolPoint = _patrolPath.GetNextPatrolPoint(0);
            }
            _startPosition = _transform.position;
            _playerTransform = FindObjectOfType<PlayerController>().transform;
            _playerCombatTarget = FindObjectOfType<PlayerController>().GetComponent<CombatTarget>();
            _isAttack = false;
        }

        protected override void Start()
        {
            base.Start();
            if (!_patrolPath) 
                return;
            
            StartMoveAction(_nextPatrolPoint.Position);
            _currentPatrolPointIndex = 1;
        }

        protected override void Update()
        {
            base.Update();
            
            float distanceToPlayer = GetDistanceToPlayer();
            if (distanceToPlayer < _attackRange && !_isAttack)
            {
                StartFightAction(_playerCombatTarget);
                _isAttack = true;
            }
            else if (distanceToPlayer < _chaseDistance && distanceToPlayer > _attackRange)
            {
                StartMoveAction(_playerTransform.position);
                _isAttack = false;
                _patrolTimer = 0;
            }
            else if (distanceToPlayer > _chaseDistance 
                     && _returnToStartPosition && _patrolTimer >= _patrolTime && !_patrolPath)
            {
                StartMoveAction(_startPosition);
                _isAttack = false;
                _patrolTimer = 0;
            }
            else if(_patrolPath && GetDistanceToNextPatrolPoint() <= _patrolPointTolerance 
                    && _patrolTimer >= _patrolTime)
            {
                
                GetNextPatrolPoint();
                StartMoveAction(_nextPatrolPoint.Position);
                _isAttack = false;
                _patrolTimer = 0;
            }
            else if(_patrolTimer >= _patrolTime && _patrolPath)
            {
                StartMoveAction(_nextPatrolPoint.Position);
                _isAttack = false;
            }

            _patrolTimer += Time.deltaTime;

        }

        private float GetDistanceToPlayer() => 
            Vector3.Distance(_transform.position, _playerTransform.position);

        private float GetDistanceToNextPatrolPoint() => 
            Vector3.Distance(_transform.position, _nextPatrolPoint.Position);

        private void GetNextPatrolPoint()
        {
            if (_currentPatrolPointIndex >= _patrolPath.PatrolPointCount)
                _currentPatrolPointIndex = 0;
            _nextPatrolPoint = _patrolPath.GetNextPatrolPoint(_currentPatrolPointIndex);
            ++_currentPatrolPointIndex;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
            Gizmos.color = Color.white;
        }

        
    }
}