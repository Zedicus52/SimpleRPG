using UnityEngine;

namespace SimpleRPG.Enemy
{
    public class PatrolPath : MonoBehaviour
    {
        public int PatrolPointCount => _patrolPoints.Length;
        
        [SerializeField] private PatrolPoint[] _patrolPoints;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < _patrolPoints.Length; i++)
            {
                Gizmos.DrawLine(_patrolPoints[i].Position, GetNextPatrolPoint(i).Position);
            }
        }

        public PatrolPoint GetNextPatrolPoint(int currentIndex)
        {
            if (currentIndex+1 >= _patrolPoints.Length)
                return _patrolPoints[0];

            return _patrolPoints[currentIndex + 1];
        }

        public PatrolPoint GetFirstPatrolPoint()
        {
            return _patrolPoints[0];
        }
    }
}