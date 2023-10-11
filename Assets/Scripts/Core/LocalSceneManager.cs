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
        public  void LoadScene()
        {
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            DontDestroyOnLoad(this);
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);
            GameContext.Instance.Load();
            Destroy(gameObject);
        }

        private int _sceneToLoad;

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