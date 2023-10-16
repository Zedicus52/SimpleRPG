namespace SimpleRPG.DataPersistence.Data
{
    [System.Serializable]
    public class PlayerStatsModel
    {
        public float MaxHealth;
        public float StrengthMultiplier;
        public float AgilityMultiplier;
        public float MovementSpeed;

        public PlayerStatsModel()
        {
            MaxHealth = 100f;
            StrengthMultiplier = 0.0001f;
            AgilityMultiplier = 0.0001f;
            MovementSpeed = 8f;
        }
    }
}