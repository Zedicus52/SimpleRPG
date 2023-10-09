using System.Threading.Tasks;
using SimpleRPG.DataPersistence;

namespace SimpleRPG.Abstraction
{
    public interface ISaver
    {
        Task LoadGame();
        void StartNewGame();
        void SaveGame();
        void RegisterObject(IDataPersistence dataPersistence);
        void UnRegisterObject(IDataPersistence dataPersistence);
    }
}