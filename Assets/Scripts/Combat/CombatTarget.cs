using System;
using UnityEngine;

namespace SimpleRPG.Combat
{
    [RequireComponent(typeof(Animator))]
    public class CombatTarget : MonoBehaviour
    {
        public bool IsDead => _health.IsDead;
        public Health CharacterHealth => _health;
        
        [Header("Health Settings")] 
        [SerializeField] private float _maxHealth;

        [Header("Experience settings")] 
        [SerializeField] private int _dropExperience;
        
        private Health _health;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            CreateHealth(_maxHealth);
        }

        public void TakeDamage(float damage) => _health.TakeDamage(damage);

        public void CreateHealth(float health) => 
            _health = new Health(health, _animator, _dropExperience);
    }
}