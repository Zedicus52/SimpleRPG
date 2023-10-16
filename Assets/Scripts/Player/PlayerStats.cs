using SimpleRPG.ScriptableObjects;
using UnityEngine;

namespace SimpleRPG.Player
{
    public class PlayerStats
    {
        public float MaxHealth { get; private set; }
        public float StrengthMultiplier { get; private set; }
        public float MaxMovementSpeed { get; private set; }
        public float AgilityMultiplier { get; private set; }
        public int AvailableSkillPoints { get; private set; }
        public int CurrentExperience { get; private set; }
        public int CurrentLevel { get; private set; }

        private readonly PlayerStatsPattern _playerStatsPattern;
        private readonly int _experienceToNewLevel;
        private readonly int _skillsPointsForNewLevel;


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

        public void SetCurrentLevel(int level) => CurrentLevel = level;
        public void SetExperience(int experience) => CurrentExperience = experience;
        public void SetAvailableSkillPoints(int skillPoints) => AvailableSkillPoints = skillPoints;

        public void AddExperience(int experience)
        {
            CurrentExperience += experience;
            
            if (CurrentExperience >= CurrentLevel * _experienceToNewLevel)
            {
                AvailableSkillPoints += _skillsPointsForNewLevel;
                CurrentExperience -= CurrentLevel * _experienceToNewLevel;
                ++CurrentLevel;
            }
        }

        public float IncreaseHealth()
        {
            if (!HasSkillPoints()) return MaxHealth;
            
            MaxHealth += _playerStatsPattern.HealthStep;
            --AvailableSkillPoints;
            Debug.Log($"Increase max health {MaxHealth}");
            return MaxHealth;
        }

        public float IncreaseStrength()
        {
            if (!HasSkillPoints()) return StrengthMultiplier;
            
            StrengthMultiplier += _playerStatsPattern.StrengthStep;
            --AvailableSkillPoints;
            return StrengthMultiplier;
        }

        public float IncreaseMovementSpeed()
        {
            if (!HasSkillPoints()) return MaxMovementSpeed;
            
            MaxMovementSpeed += _playerStatsPattern.MovementSpeedStep;
            --AvailableSkillPoints;
            return MaxMovementSpeed;
        }

        public float IncreaseAgility()
        {
            if(!HasSkillPoints()) return AgilityMultiplier;
            
            AgilityMultiplier += _playerStatsPattern.AgilityStep;
            --AvailableSkillPoints;
            return AgilityMultiplier;
        }

        private bool HasSkillPoints() => AvailableSkillPoints > 0;



    }
}