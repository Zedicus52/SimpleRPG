using System;
using System.Collections;
using UnityEngine;

namespace SimpleRPG.Core
{
    public class Recoverable : MonoBehaviour
    {
        [SerializeField] private float _restoreTime;

        private MeshRenderer _renderer;

      
        public void StartRestore(MeshRenderer obj)
        {
            StartCoroutine(WaitRestoreTime(obj));
        }
        public void StartRestore(GameObject obj)
        {
            StartCoroutine(WaitRestoreTime(obj));
        }

        private IEnumerator WaitRestoreTime(GameObject obj)
        {
            obj.SetActive(false);
            yield return new WaitForSeconds(_restoreTime);
            obj.SetActive(true);
        }
        
        private IEnumerator WaitRestoreTime(MeshRenderer renderer)
        {
            renderer.enabled = false;
            yield return new WaitForSeconds(_restoreTime);
            renderer.enabled = true;
        }
        
        
    }
}