using System;
using UnityEngine;

namespace SimpleRPG.Enemy
{
    public class PatrolPoint : MonoBehaviour
    {
        public Vector3 Position => transform.position;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(Position, 0.5f);
            Gizmos.color = Color.white;
        }
    }
}