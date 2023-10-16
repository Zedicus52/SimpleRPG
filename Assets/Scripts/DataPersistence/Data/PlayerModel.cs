namespace SimpleRPG.DataPersistence.Data
{
    [System.Serializable]
    public class PlayerModel
    {
        public float CurrentHealth;
        public SerializableVector3 Position;
        public SerializableVector3 Rotation;
        public int LastSceneId;
        public string WeaponId;
        public int CurrentExperience;
        public int CurrentLevel;
        public int AvailableSkillPoints;
        public PlayerStatsModel Stats;

        public PlayerModel()
        {
            Stats = new PlayerStatsModel();
            CurrentLevel = 1;
            CurrentExperience = 0;
            AvailableSkillPoints = 0;
            WeaponId = "5ad94a79-cc9f-4d8d-b6df-aa8914297b7b";
        }

    }
}