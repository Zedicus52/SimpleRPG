using UnityEngine;

namespace SimpleRPG.Combat
{
    public class Health
    {
        public bool IsDead => _currentHealth <= 0;
        
        private readonly float _maxHealth;
        private readonly Animator _animator;
        
        private float _currentHealth;

        private readonly int _die = Animator.StringToHash("Die");

        public Health(float maxHealth, Animator animator)
        {
            _maxHealth = maxHealth;
            _animator = animator;
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if(IsDead)
                return;
            
            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
            
            if(IsDead)
                _animator.SetTrigger(_die);
        }
    }
}