using System.Collections;
using SimpleRPG.Core;
using SimpleRPG.Enums;
using SimpleRPG.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimpleRPG.SceneManagement
{
    [RequireComponent(typeof(BoxCollider))]
    public class LevelTransition : MonoBehaviour
    {
        public PortalCode Code => _portalCode;
        public Transform SpawnPoint => _spawnPoint;
        
        [SerializeField] private PortalCode _portalCode;
        [SerializeField] private int _indexSceneToLoad;
        [SerializeField] private Transform _spawnPoint;
        

        private void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController playerController))
            {
                StartCoroutine(LoadScene());
            }
        }

        private IEnumerator LoadScene()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(_indexSceneToLoad);
            GameContext.Instance.InitializeLoadedScene(this);
            Destroy(gameObject);
        }
    }
}