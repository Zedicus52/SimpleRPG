using System;
using SimpleRPG.DataPersistence;
using SimpleRPG.DataPersistence.Data;
using SimpleRPG.Player;
using UnityEngine;
using UnityEngine.Playables;

namespace SimpleRPG.Cinematic
{
    [RequireComponent(typeof(BoxCollider))]
    public class CinematicTriggerZone : MonoBehaviour, IDataPersistence
    {
        public static event Action CinematicStarted;
        public static event Action CinematicEnded;
        
        [SerializeField] private PlayableDirector _cinematic;

        private bool _isPlayed;

        private void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
            _isPlayed = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController player) && _isPlayed == false)
            {
                _isPlayed = true;
                _cinematic.Play();
                _cinematic.stopped += OnCinematicStopped;
                CinematicStarted?.Invoke();
            }
        }

        private void OnCinematicStopped(PlayableDirector cinematic)
        {
            cinematic.stopped -= OnCinematicStopped;
            CinematicEnded?.Invoke();
        }

        public void LoadData(GameData gameData)
        {
            
        }

        public void SaveData(ref GameData gameData)
        {
            
        }
    }
}