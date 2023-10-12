using System;
using UnityEngine;

namespace SimpleRPG.Combat
{
    [RequireComponent(typeof(BoxCollider))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private bool _isHoming;

        private Transform _transform;
        private CombatTarget _target;
        private float _damage;

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
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CombatTarget target))
            {
                if (_target == target)
                {
                    target.TakeDamage(_damage);
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