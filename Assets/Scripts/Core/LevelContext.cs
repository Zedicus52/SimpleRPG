using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleRPG.Core
{
    public class LevelContext : MonoBehaviour
    {
        public static LevelContext Instance { get; private set; }

        public Transform SpawnPoint => _spawnPoint;
        
        [SerializeField] private List<Checkpoint> _levelCheckpoints;
        [SerializeField] private Transform _spawnPoint;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }
            Destroy(gameObject);
        }
    }
}