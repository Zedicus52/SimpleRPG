using System;
using System.Collections;
using SimpleRPG.DataPersistence;
using SimpleRPG.DataPersistence.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimpleRPG.Core
{
    public class LocalSceneManager : MonoBehaviour, IDataPersistence
    {
        private int _sceneToLoad = 1;

        public void LoadLastScene()
        {
            StartCoroutine(Load());
        }

        public void StartNewGame()
        {
            StartCoroutine(StartGame());
        }

        private IEnumerator Load()
        {
            DontDestroyOnLoad(this);
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);
            GameContext.Instance.LoadLastSave();
            Destroy(gameObject);
        }

        private IEnumerator StartGame()
        {
            DontDestroyOnLoad(this);
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);
            GameContext.Instance.StartNewGame();
            Destroy(gameObject);
        }


        private void OnEnable()
        {
            GameContext.Instance.Saver.RegisterObject(this);
        }

        private void OnDisable()
        {
            GameContext.Instance.Saver.UnRegisterObject(this);

        }

        public void LoadData(GameData gameData)
        {
            _sceneToLoad = gameData.Player.LastSceneId;
        }

        public void SaveData(ref GameData gameData)
        {
        }
    }
}