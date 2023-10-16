using System;
using SimpleRPG.Player;
using UnityEngine;

namespace SimpleRPG.Combat
{
    [RequireComponent(typeof(SphereCollider))]
    public class HealthPotion : MonoBehaviour
    {
        [SerializeField] private float _restoreHealth;

        private void Awake()
        {
            GetComponent<SphereCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController player))
            {
                player.RestoreHealth(_restoreHealth);
                Destroy(gameObject);
            }
        }
    }
}