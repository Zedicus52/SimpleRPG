namespace SimpleRPG.DataPersistence.Data
{
    [System.Serializable]
    public class GameData
    {
        public PlayerModel Player;
        
        public GameData()
        {
            Player = new PlayerModel() {LastSceneId = 1};
        }
    }
}