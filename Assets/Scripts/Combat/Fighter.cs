using SimpleRPG.Abstraction;
using SimpleRPG.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

namespace SimpleRPG.Combat
{
    public class Fighter : IAction
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly float _attackFrequency;
        private readonly Animator _animator;
        private readonly CombatTarget _target;
        private readonly Transform _leftHand;
        private readonly Transform _rightHand;
        private  WeaponSO _currentWeapon;
        
        private readonly int _attack = Animator.StringToHash("Attack");
        private readonly int _cancelAttack = Animator.StringToHash("CancelAttack");

        private CombatTarget _currentTarget;
        private float _lastAttackTime;
        
        public Fighter(NavMeshAgent agent, Animator animator, 
            float attackFrequency, WeaponSO weaponSo,  CombatTarget target, 
            Transform rightHand, Transform leftHand)
        {
            _rightHand = rightHand;
            _leftHand = leftHand;
            _navMeshAgent = agent;
            _attackFrequency = attackFrequency;
            _target = target;
            _animator = animator;
            _currentWeapon = weaponSo;
        }
        public void Attack()
        {
            if (_currentTarget == null)
                return;
            
            if (Vector3.Distance(_currentTarget.transform.position, _navMeshAgent.transform.position) 
                <= _currentWeapon.AttackRange)
            {
                if (_currentTarget.IsDead == false)
                {
                    if (_currentWeapon.HasProjectile())
                        _currentWeapon.LaunchProjectile(_leftHand, _rightHand, _currentTarget);
                    else
                        _currentTarget.TakeDamage(_currentWeapon.Damage);
                }
            }
            
        }

        public void SetWeapon(WeaponSO weaponSo) => _currentWeapon = weaponSo;

        public void SetTarget(CombatTarget target)
        {
            if (!target || _target.IsDead) 
                return;
            
            _currentTarget = target;
            _lastAttackTime = 0;
            MoveToTarget();
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
            _animator.SetTrigger(_cancelAttack);
            _animator.ResetTrigger(_attack);
            _currentTarget = null;
        }


        public void Update()
        {
            if(_target.IsDead)
                return;
            
            _lastAttackTime += Time.deltaTime; 
            
            if (!_currentTarget)
                return;


            if (Vector3.Distance(_currentTarget.transform.position, _navMeshAgent.transform.position) <=
                _currentWeapon.AttackRange)
            {
                _navMeshAgent.isStopped = true;
                _navMeshAgent.transform.LookAt(_currentTarget.transform);
                if (_lastAttackTime >= _attackFrequency && _currentTarget.IsDead == false)
                {
                    _animator.ResetTrigger(_cancelAttack);
                    _animator.SetTrigger(_attack);
                    _lastAttackTime = 0;
                }
            }
        }

        private void MoveToTarget()
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_currentTarget.transform.position);
        }
    }
}   