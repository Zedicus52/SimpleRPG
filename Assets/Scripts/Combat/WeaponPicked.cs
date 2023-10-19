using System;
using SimpleRPG.Core;
using SimpleRPG.Player;
using SimpleRPG.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleRPG.Combat
{
    [RequireComponent(typeof(SphereCollider), typeof(Recoverable))]
    public class WeaponPicked : MonoBehaviour
    {
        [SerializeField] private WeaponSO _weapon;

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
                player.SetWeapon(_weapon);
                _pickUpEvent?.Invoke();
                _recoverable.StartRestore(GetComponent<MeshRenderer>());
            }
        }
    }
}