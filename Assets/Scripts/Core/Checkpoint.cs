using System;
using SimpleRPG.Player;
using SimpleRPG.ScriptableObjects;
using UnityEngine;

namespace SimpleRPG.Core
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class Checkpoint : MonoBehaviour
    {
        public string Id => _checkpointData.Id;
        public Transform SpawnPoint => _spawnPoint;
        
        [SerializeField] private CheckpointSO _checkpointData;
        [SerializeField] private Transform _spawnPoint;
        private bool _isReached;

        private void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
            var rigid = GetComponent<Rigidbody>();
            rigid.useGravity = false;
            rigid.isKinematic = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController player) && _isReached == false)
            {
                _isReached = true;
                _checkpointData.SetIsReached();
                player.SetLastCheckpoint(this);
            }
        }

        public void SetIsReached(bool isReached = true) => _isReached = isReached;


    }
}