using SimpleRPG.Combat;
using UnityEngine;

namespace SimpleRPG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG/Weapon", order = 0)]
    public class WeaponSO : ScriptableObject
    {
        public float Damage => _damage;
        public float AttackRange => _attackRange;
        
        [SerializeField] private AnimatorOverrideController _animatorOverride;
        [SerializeField] private WeaponHolder _weaponPrefab;
        [SerializeField] private float _damage;
        [SerializeField] private float _attackRange;
        [SerializeField] private bool _isRightHanded;

        public void SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (_weaponPrefab != null)
            {
                Instantiate(_weaponPrefab, _isRightHanded ? rightHand : leftHand);
            }

            if (_animatorOverride != null)
            {
                animator.runtimeAnimatorController = _animatorOverride;
            }
        }
    }
}