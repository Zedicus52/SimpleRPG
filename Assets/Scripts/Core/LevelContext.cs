using System;
using System.Collections.Generic;
using System.Linq;
using SimpleRPG.Cinematic;
using SimpleRPG.ScriptableObjects;
using SimpleRPG.UI;
using UnityEngine;

namespace SimpleRPG.Core
{
    public class LevelContext : MonoBehaviour
    {
        public static LevelContext Instance { get; private set; }

        public Transform SpawnPoint => _spawnPoint;
        public StatsPanel StatsUI => _statsPanel;

        public Player_IA PlayerInput => _playerInput;
        
        [Header("Level objects")]
        [SerializeField] private List<Checkpoint> _levelCheckpoints;
        [SerializeField] private List<CinematicTriggerZone> _levelCinematic;

        [Header("Data Objects")]
        [SerializeField] private List<CinematicSO> _cinematicSos;
        [SerializeField] private List<CheckpointSO> _checkpointsSos;

        [Header("UI")] 
        [SerializeField] private StatsPanel _statsPanel;
        
        [Header("Other")]
        [SerializeField] private Transform _spawnPoint;

        private Player_IA _playerInput;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _playerInput = new Player_IA();
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

                    var checkpoint = _levelCheckpoints.FirstOrDefault(x => x.Id.Equals(data.Id));
                    if (checkpoint != null)
                        checkpoint.SetIsReached(data.IsReached);
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
                    cinematic.SetIsPlayed(data.IsPlayed);
            }
        }

        public void ResetCheckPoints()
        {
            foreach (var levelCheckpoint in _checkpointsSos)
            {
               levelCheckpoint.SetIsReached(false);
            }
        }

        public void ResetCinematics()
        {
            foreach (var cinematic in _cinematicSos)
            {
                cinematic.SetIsPlayed(false);
            }
        }

        public Checkpoint GetCheckpointById(string id)
        {
            return _levelCheckpoints.FirstOrDefault(x => x.Id.Equals(id));
        }
    }
}