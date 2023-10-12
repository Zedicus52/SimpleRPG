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
        [SerializeField] private Projectile _projectile;
        public WeaponHolder SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (_animatorOverride != null)
            {
                animator.runtimeAnimatorController = _animatorOverride;
            }
            
            if (_weaponPrefab != null)
            {
                return Instantiate(_weaponPrefab, GetTransform(leftHand, rightHand));
            }

            return null;
        }

        public bool HasProjectile() => _projectile != null;

        public void LaunchProjectile(Transform leftHand, Transform rightHand, CombatTarget target)
        {
            if(HasProjectile() == false)
                return;

            Transform transform = GetTransform(leftHand, rightHand);
            Projectile proj = Instantiate(_projectile, transform.position, Quaternion.identity);
            proj.SetTarget(target);
            proj.SetDamage(_damage);
        }

        private Transform GetTransform(Transform leftHand, Transform rightHand) => _isRightHanded ? rightHand : leftHand;
    }
}