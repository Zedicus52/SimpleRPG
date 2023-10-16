using SimpleRPG.ScriptableObjects;

namespace SimpleRPG.Player
{
    public class PlayerStats
    {
        public float MaxHealth { get; private set; }
        public float StrengthMultiplier { get; private set; }
        public float MaxMovementSpeed { get; private set; }
        public float AgilityMultiplier { get; private set; }

        private readonly PlayerStatsPattern _playerStatsPattern;
        private readonly int _experienceToNewLevel;
        private readonly int _skillsPointsForNewLevel;

        private int _availableSkillsPoints;
        private int _currentExperience;
        private int _currentLevel;


        public PlayerStats(float maxHealth, float strengthMultiplier,
            float maxMovementSpeed, float agilityMultiplier, 
            PlayerStatsPattern playerStatsPattern, int experienceToNewLevel, int skillsPointsForNewLevel) 
            : this(experienceToNewLevel, skillsPointsForNewLevel, playerStatsPattern)
        {
            MaxHealth = maxHealth;
            StrengthMultiplier = strengthMultiplier;
            MaxMovementSpeed = maxMovementSpeed;
            AgilityMultiplier = agilityMultiplier;
        }

        public PlayerStats(int experienceToNewLevel, int skillsPointsForNewLevel, PlayerStatsPattern pattern)
        {
            _experienceToNewLevel = experienceToNewLevel;
            _skillsPointsForNewLevel = skillsPointsForNewLevel;
            _playerStatsPattern = pattern;
        }

        public void AddExperience(int experience)
        {
            _currentExperience += experience;
            
            if (_currentExperience >= _currentLevel * _experienceToNewLevel)
            {
                _availableSkillsPoints += _skillsPointsForNewLevel;
                _currentExperience -= _currentLevel * _experienceToNewLevel;
                ++_currentLevel;
            }
        }

        public float IncreaseHealth()
        {
            if (!HasSkillPoints()) return MaxHealth;
            
            MaxHealth += _playerStatsPattern.HealthStep;
            --_availableSkillsPoints;
            return MaxHealth;
        }

        public float IncreaseStrength()
        {
            if (!HasSkillPoints()) return StrengthMultiplier;
            
            StrengthMultiplier += _playerStatsPattern.StrengthStep;
            --_availableSkillsPoints;
            return StrengthMultiplier;
        }

        public float IncreaseMovementSpeed()
        {
            if (!HasSkillPoints()) return MaxMovementSpeed;
            
            MaxMovementSpeed += _playerStatsPattern.MovementSpeedStep;
            --_availableSkillsPoints;
            return MaxMovementSpeed;
        }

        public float IncreaseAgility()
        {
            if(!HasSkillPoints()) return AgilityMultiplier;
            
            AgilityMultiplier += _playerStatsPattern.AgilityStep;
            --_availableSkillsPoints;
            return AgilityMultiplier;
        }

        private bool HasSkillPoints() => _availableSkillsPoints > 0;



    }
}