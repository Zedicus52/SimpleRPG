using System;
using System.Collections;
using System.Collections.Generic;
using SimpleRPG.Core;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

namespace SimpleRPG.Combat
{
    [RequireComponent(typeof(BoxCollider))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private bool _isHoming;
        [SerializeField] private ImpactEffect _impactEffect;

        private Transform _transform;
        private CombatTarget _target;
        private float _damage;
        private readonly float _lifeTime = 10f;
        private float _currentTime;

        private void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
            _transform = GetComponent<Transform>();
        }

        private void Start() => _transform.LookAt(GetDestination());

        private void Update()
        {
            Vector3 destination = GetDestination();
            if(destination == Vector3.zero)
                return;

            if(_isHoming)
                _transform.LookAt(GetDestination());
            
            _transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
            
            if(_currentTime >= _lifeTime)
                Destroy(gameObject);
            _currentTime += Time.deltaTime;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CombatTarget target))
            {
                if (_target == target)
                {
                    target.TakeDamage(_damage);
                    if(_impactEffect)
                        Instantiate(_impactEffect, other.transform.position, UnityEngine.Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }

        private Vector3 GetDestination()
        {
            if (!_target)
                return Vector3.zero;

            if (_target.TryGetComponent(out CapsuleCollider collider))
            {
                return _target.transform.position + Vector3.up * (collider.height * 0.5f);
            }
            return Vector3.zero;
        }

        public void SetTarget(CombatTarget target) => _target = target;
        public void SetDamage(float damage) => _damage = damage;
        

    }
}