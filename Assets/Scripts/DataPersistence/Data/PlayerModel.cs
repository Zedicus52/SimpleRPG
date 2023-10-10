namespace SimpleRPG.DataPersistence.Data
{
    [System.Serializable]
    public class PlayerModel
    {
        public float Health;
        public SerializableVector3 Position;
        public SerializableVector3 Rotation;
        public int LastSceneId;

    }
}