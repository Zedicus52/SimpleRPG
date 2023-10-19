using System;
using System.Collections;
using UnityEngine;

namespace SimpleRPG.Core
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] private float _lifeTime;

        private void Start()
        {
            StartCoroutine(LifeTimeCoroutine());
        }

        private IEnumerator LifeTimeCoroutine()
        {
            yield return new WaitForSeconds(_lifeTime);
            Destroy(gameObject);
        }
    }
}