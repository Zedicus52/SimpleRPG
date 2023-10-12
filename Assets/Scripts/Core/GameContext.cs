using System;
using System.Collections.Generic;
using System.Linq;
using SimpleRPG.Abstraction;
using SimpleRPG.DataPersistence;
using SimpleRPG.DataPersistence.Data;
using SimpleRPG.Player;
using SimpleRPG.SceneManagement;
using SimpleRPG.ScriptableObjects;
using UnityEngine;

namespace SimpleRPG.Core
{
    public class GameContext : MonoBehaviour
    {
        public static GameContext Instance { get; private set; }

        public ISaver Saver { get; private set; }
        
        [SerializeField] private PlayerController _playerPrefab;
        
        [Header("Weapons")] 
        [SerializeField] private List<WeaponSO> _allWeapons;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Saver = new LocalSaver("file", false);
                DontDestroyOnLoad(this);
                return;
            }
            Destroy(gameObject);
        }

        private void Start()
        {
            Saver.LoadGame();
        }

        private void OnApplicationQuit()
        {
            //Saver.SaveGame();
        }


        public void InitializeLoadedScene(LevelTransition currentPortal)
        {
            LevelTransition neededPortal = GetNeededPortal(currentPortal);
            if (neededPortal)
                SpawnPlayer(neededPortal.SpawnPoint.position, Quaternion.identity);
        }

        private LevelTransition GetNeededPortal(LevelTransition currentPortal)
        {
            var portals = 
                FindObjectsByType<LevelTransition>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var portal in portals)
            {
                if(portal == currentPortal) continue;
                if(portal.Code != currentPortal.Code) continue;
                return portal;
            }

            return null;
        }

        public void Load()
        {
            GameData data = Saver.GetCurrentSave();
            if (data.Player.Position != null && data.Player.Rotation != null)
            {
                var pos = data.Player.Position;
                var rotation = data.Player.Rotation;
                var newPos = new Vector3(pos.X, pos.Y, pos.Z);
                if (newPos == Vector3.zero)
                    newPos = LevelContext.Instance.SpawnPoint.position;
                SpawnPlayer(newPos, Quaternion.Euler(rotation.X, rotation.Y, rotation.Z));
            }
            
        }

        public WeaponSO GetWeaponById(string id) => 
            _allWeapons.FirstOrDefault(x => x.Id.Equals(id));

        private void SpawnPlayer(Vector3 position, Quaternion rotation)
        {
            PlayerController player = Instantiate(_playerPrefab, position, rotation);
            player.LoadData(Saver.GetCurrentSave());
        }
    }
}