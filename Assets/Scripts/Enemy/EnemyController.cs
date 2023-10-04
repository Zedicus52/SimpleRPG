using SimpleRPG.Combat;
using SimpleRPG.Player;
using UnityEngine;
using CharacterController = SimpleRPG.Core.CharacterController;

namespace SimpleRPG.Enemy
{
    public class EnemyController : CharacterController
    {
        [Header("Enemy chase settings")]
        [SerializeField] private float _chaseDistance;

        private Transform _playerTransform;
        private CombatTarget _playerCombatTarget;

        protected override void Awake()
        {
            base.Awake();
            _playerTransform = FindObjectOfType<PlayerController>().transform;
            _playerCombatTarget = FindObjectOfType<PlayerController>().GetComponent<CombatTarget>();
        }

        protected override void Update()
        {
            base.Update();

            if (Vector3.Distance(_transform.position, _playerTransform.position) < _chaseDistance)
            {
                StartFightAction(_playerCombatTarget);
            }
        }
    }
}