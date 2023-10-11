namespace SimpleRPG.DataPersistence.Data
{
    [System.Serializable]
    public class SerializableVector3
    {
        public float X;
        public float Y;
        public float Z;

        public SerializableVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}