using SimpleRPG.DataPersistence.Data;

namespace SimpleRPG.DataPersistence
{
    public interface IDataPersistence
    {
        void LoadData(GameData gameData);

        void SaveData(ref GameData gameData);
    }
}