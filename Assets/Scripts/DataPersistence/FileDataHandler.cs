using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SimpleRPG.DataPersistence.Data;
using UnityEngine;

namespace SimpleRPG.DataPersistence
{
    public class FileDataHandler
    {
        public string SaveDirectoryPath => Path.Combine(_directoryPath, _fileName);

        private readonly string _directoryPath;
        private readonly string _fileName;
        private readonly bool _useEncryption;
        private readonly string _encryptionCodeWord;

        public FileDataHandler(string directoryPath, string fileName, bool useEncryption)
        {
            var fileExtension = ".zed";
            _encryptionCodeWord = "someWord";
            _directoryPath = directoryPath;
            _fileName = fileName + fileExtension;
            _useEncryption = useEncryption;
            
        }
        
        public async Task<GameData> Load()
        {
            string fullPath = Path.Combine(_directoryPath, _fileName);
            GameData loadedData = null;
            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    
                    await using FileStream stream = new FileStream(fullPath, FileMode.Open);
                    using StreamReader reader = new StreamReader(stream);
                    dataToLoad = await reader.ReadToEndAsync();

                    if (_useEncryption)
                        dataToLoad = EncryptDecrypt(dataToLoad);

                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to load file at path: " 
                                   + fullPath  + " and backup did not work.\n" + e);
                }
            }

            return loadedData;
        }
        
        public async Task Save(GameData data)
        {
            string fullPath = Path.Combine(_directoryPath, _fileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? string.Empty);
                string dataToStore = JsonUtility.ToJson(data, true);

                if (_useEncryption)
                    dataToStore = EncryptDecrypt(dataToStore);

                await using FileStream stream = new FileStream(fullPath, FileMode.Create);
                await using StreamWriter writer = new StreamWriter(stream);
                await writer.WriteAsync(dataToStore);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }
        
        private string EncryptDecrypt(string data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sb.Append((char)(data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]));

            return sb.ToString();
        }
    }
}