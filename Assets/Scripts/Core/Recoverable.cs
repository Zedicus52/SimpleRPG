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

        private IEnumerator WaitRestoreTime(MeshRenderer renderer)
        {
            renderer.enabled = false;
            yield return new WaitForSeconds(_restoreTime);
            renderer.enabled = true;
        }
        
        
    }
}