using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleRPG.Abstraction;
using SimpleRPG.DataPersistence.Data;
using UnityEngine;

namespace SimpleRPG.DataPersistence
{
    public class LocalSaver : ISaver
    {
        private GameData _gameData;
        private readonly List<IDataPersistence> _dataPersistenceObjects;
        private readonly FileDataHandler _dataHandler;

        public LocalSaver(string fileName, bool useEncryption)
        {
            _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
            _dataPersistenceObjects = new List<IDataPersistence>();
        }
        
        public async Task LoadGame()
        {
            _gameData = await _dataHandler.Load();

            if (_gameData == null)
            {
                StartNewGame();
                return;
            }

            foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
                dataPersistenceObj.LoadData(_gameData);
        }

        public void StartNewGame() => _gameData = new GameData();

        public async void SaveGame()
        {
            foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
                dataPersistenceObj.SaveData(ref _gameData);

            await _dataHandler.Save(_gameData);
        }

        public void RegisterObject(IDataPersistence dataPersistence) =>
            _dataPersistenceObjects.Add(dataPersistence);

        public void UnRegisterObject(IDataPersistence dataPersistence) =>
            _dataPersistenceObjects.Remove(dataPersistence);

        public GameData GetCurrentSave() => _gameData;

    }
}