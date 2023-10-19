using UnityEngine;

namespace SimpleRPG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Player Stats Pattern", menuName = "RPG/Player Stats", order = 0)]
    public class PlayerStatsPattern : ScriptableObject
    {
        public float HealthStep => _healthStep;
        public float MovementSpeedStep => _movementSpeedStep;
        public float StrengthStep => _strengthStep;
        public float AgilityStep => _agilityStep;
        
        [SerializeField] private float _healthStep;
        [SerializeField] private float _movementSpeedStep;
        [SerializeField] private float _strengthStep;
        [SerializeField] private float _agilityStep;
    }
}