using System;
using SimpleRPG.Player;
using SimpleRPG.ScriptableObjects;
using UnityEngine;

namespace SimpleRPG.Combat
{
    [RequireComponent(typeof(SphereCollider))]
    public class WeaponPicked : MonoBehaviour
    {
        [SerializeField] private WeaponSO _weapon;
        private void Awake()
        {
            GetComponent<SphereCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController player))
            {
                player.SetWeapon(_weapon);
                Destroy(gameObject);
            }
        }
    }
}