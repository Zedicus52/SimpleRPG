using SimpleRPG.ScriptableObjects;

namespace SimpleRPG.Player
{
    public class PlayerStats
    {
        private float _maxHealth;
        private float _strengthMultiplier;
        private float _maxMovementSpeed;
        private float _agilityMultiplier;

        private readonly PlayerStatsPattern _playerStatsPattern;

        public PlayerStats(float maxHealth, float strengthMultiplier,
            float maxMovementSpeed, float agilityMultiplier, PlayerStatsPattern playerStatsPattern)
        {
            _maxHealth = maxHealth;
            _strengthMultiplier = strengthMultiplier;
            _maxMovementSpeed = maxMovementSpeed;
            _agilityMultiplier = agilityMultiplier;
            _playerStatsPattern = playerStatsPattern;
        }

        public void IncreaseHealth() => _maxHealth += _playerStatsPattern.HealthStep;
        public void IncreaseStrength() => _strengthMultiplier += _playerStatsPattern.StrengthStep;
        public void IncreaseMovementSpeed() => _maxMovementSpeed += _playerStatsPattern.MovementSpeedStep;
        public void IncreaseAgility() => _agilityMultiplier += _playerStatsPattern.AgilityStep;



    }
}