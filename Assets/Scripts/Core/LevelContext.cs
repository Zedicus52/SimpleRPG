using System;
using System.Collections.Generic;
using System.Linq;
using SimpleRPG.Cinematic;
using SimpleRPG.ScriptableObjects;
using UnityEngine;

namespace SimpleRPG.Core
{
    public class LevelContext : MonoBehaviour
    {
        public static LevelContext Instance { get; private set; }

        public Transform SpawnPoint => _spawnPoint;
        
        [SerializeField] private List<Checkpoint> _levelCheckpoints;
        [SerializeField] private List<CinematicTriggerZone> _levelCinematic;

        [SerializeField] private List<CinematicSO> _cinematicSos;
        [SerializeField] private List<CheckpointSO> _checkpointsSos;
        
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

        private void Start()
        {
            InitializeCheckpoints();
            InitializeCinematic();
        }

        private void InitializeCheckpoints()
        {
            if(_checkpointsSos.Count != _levelCheckpoints.Count)
                return;

            foreach (var data in _checkpointsSos)
            {
                if (data.IsReached)
                {
                    var checkpoint = _levelCheckpoints.FirstOrDefault(x => x.Id.Equals(data.Id));
                    if (checkpoint != null)
                        checkpoint.SetIsReached();
                }
            }
        }

        private void InitializeCinematic()
        {
            if(_cinematicSos.Count != _levelCinematic.Count)
                return;

            foreach (var data in _cinematicSos)
            {
                var cinematic = 
                    _levelCinematic.FirstOrDefault(x => x.Id.Equals(data.Id));
                if(cinematic != null)
                    cinematic.SetIsPlayed();
            }
        }
    }
}