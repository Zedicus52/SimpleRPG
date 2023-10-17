using System;
using UnityEngine;
using UnityEngine.Events;

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

        [Header("Sound effects")] 
        [SerializeField] private UnityEvent _takingDamage;
        [SerializeField] private UnityEvent _die;
        
        private Health _health;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            CreateHealth(_maxHealth, _maxHealth);
        }

        private void OnEnable()
        {
            Health.CharacterDie += OnCharacterDie;
        }

        private void OnDisable()
        {
            Health.CharacterDie -= OnCharacterDie;
        }

        private void OnCharacterDie(int obj)
        {
            _die?.Invoke();
        }

        public void TakeDamage(float damage)
        {
            _health.TakeDamage(damage);
            _takingDamage?.Invoke();
        }

        public void CreateHealth(float maxHealth, float currentHealth) => 
            _health = new Health(maxHealth,currentHealth, _animator, _dropExperience);
    }
}