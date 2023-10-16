using System;
using UnityEngine;

namespace SimpleRPG.Combat
{
    public class Health
    {

        public static event Action<int> CharacterDie;
        public bool IsDead => _currentHealth <= 0;
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        
        private readonly float _maxHealth;
        private readonly Animator _animator;
        private readonly int _dropExperience;
        
        private float _currentHealth;

        private readonly int _die = Animator.StringToHash("Die");

        public Health(float maxHealth, Animator animator, int dropExperience)
        {
            _maxHealth = maxHealth;
            _animator = animator;
            _dropExperience = dropExperience;
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if(IsDead)
                return;
            
            _currentHealth = Mathf.Max(_currentHealth - damage, 0);

            if (IsDead)
            {
                _animator.SetTrigger(_die);
                CharacterDie?.Invoke(_dropExperience);
            }
        }
    }
}