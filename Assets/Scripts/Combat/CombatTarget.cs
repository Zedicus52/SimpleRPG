using System;
using UnityEngine;

namespace SimpleRPG.Combat
{
    [RequireComponent(typeof(Animator))]
    public class CombatTarget : MonoBehaviour
    {
        public bool IsDead => _health.IsDead;
        
        [Header("Health Settings")] 
        [SerializeField] private float _maxHealth;
        
        private Health _health;

        private void Awake()
        {
            _health = new Health(_maxHealth, GetComponent<Animator>());
        }

        public void TakeDamage(float damage) => _health.TakeDamage(damage);
    }
}