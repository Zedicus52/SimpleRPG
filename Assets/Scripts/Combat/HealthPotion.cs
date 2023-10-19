using System;
using SimpleRPG.Core;
using SimpleRPG.Player;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleRPG.Combat
{
    [RequireComponent(typeof(SphereCollider), typeof(Recoverable))]
    public class HealthPotion : MonoBehaviour
    {
        [SerializeField] private float _restoreHealth;
        [SerializeField] private GameObject _renderer;

        [Header("Sound")] 
        [SerializeField] private UnityEvent _pickUpEvent;
        
        private Recoverable _recoverable;
        

        private void Awake()
        {
            GetComponent<SphereCollider>().isTrigger = true;
            _recoverable = GetComponent<Recoverable>();                

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController player))
            {
                _pickUpEvent?.Invoke();
                player.RestoreHealth(_restoreHealth);
                _recoverable.StartRestore(_renderer);

            }
        }
    }
}