using System;
using SimpleRPG.Core;
using SimpleRPG.Player;
using SimpleRPG.ScriptableObjects;
using UnityEngine;

namespace SimpleRPG.Combat
{
    [RequireComponent(typeof(SphereCollider), typeof(Recoverable))]
    public class WeaponPicked : MonoBehaviour
    {
        [SerializeField] private WeaponSO _weapon;
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
                _recoverable.StartRestore(GetComponent<MeshRenderer>());
            }
        }
    }
}