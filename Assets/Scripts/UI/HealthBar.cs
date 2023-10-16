using System;
using SimpleRPG.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleRPG.UI
{
    [RequireComponent(typeof(Slider))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private CombatTarget _target;


        private void Start()
        {
            _slider.maxValue = _target.CharacterHealth.MaxHealth;
            _slider.value = _target.CharacterHealth.MaxHealth;
            _slider.minValue = 0f;
            _target.CharacterHealth.MaxHealthChanged += OnMaxHealthChanged;
            _target.CharacterHealth.HealthChanged += OnHealthChanged;
        }
        
        private void OnDestroy()
        {
            _target.CharacterHealth.MaxHealthChanged -= OnMaxHealthChanged;
            _target.CharacterHealth.HealthChanged -= OnHealthChanged;
        }
        
        private void OnHealthChanged(float obj)
        {
            _slider.value = obj;
        }

        private void OnMaxHealthChanged(float obj)
        {
            _slider.maxValue = obj;
        }
    }
}