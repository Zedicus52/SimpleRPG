using System;
using SimpleRPG.Abstraction;
using SimpleRPG.DataPersistence;
using SimpleRPG.Player;
using SimpleRPG.SceneManagement;
using UnityEngine;

namespace SimpleRPG.Core
{
    public class GameContext : MonoBehaviour
    {
        public static GameContext Instance { get; private set; }

        public ISaver Saver { get; private set; }
        
        [SerializeField] private PlayerController _playerPrefab;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            
            Saver = new LocalSaver("file", false);
            
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            //Saver.LoadGame();
        }

        private void OnApplicationQuit()
        {
            //Saver.SaveGame();
        }


        public void InitializeLoadedScene(LevelTransition currentPortal)
        {
            LevelTransition neededPortal = GetNeededPortal(currentPortal);
            if (neededPortal)
                SpawnPlayer(neededPortal.SpawnPoint.localPosition);
        }

        private LevelTransition GetNeededPortal(LevelTransition currentPortal)
        {
            var portals = 
                FindObjectsByType<LevelTransition>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var portal in portals)
            {
                if(portal != currentPortal) continue;
                if(portal.Code != currentPortal.Code) continue;
                return portal;
            }

            return null;
        }

        private void SpawnPlayer(Vector3 position)
        {
            Instantiate(_playerPrefab, position, Quaternion.identity);
        }
    }
}