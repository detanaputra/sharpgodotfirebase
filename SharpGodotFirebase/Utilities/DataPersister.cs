using Godot;

using Newtonsoft.Json;

using System.Collections.Generic;


namespace SharpGodotFirebase.Utilities
{
    internal class DataPersister
    {
        private const string filePath = "user://SharpGodotFirebase/Data.json";
        private const string folderPath = "user://SharpGodotFirebase";

        public DataPersister()
        {

        }

        internal static DataPersister Build()
        {
            return new DataPersister();
        }

        protected static Dictionary<string, string> PersistedData { get; set; } = new Dictionary<string, string>();

        internal DataPersister AddData(string key, object value)
        {
            string serializedvalue = JsonConvert.SerializeObject(value);
            if (PersistedData.ContainsKey(key))
            {
                PersistedData[key] = serializedvalue;
            }
            else
            {
                PersistedData.Add(key, serializedvalue);
            }
            return this;
        }

        internal DataPersister RemoveData(string key)
        {
            if (PersistedData.ContainsKey(key))
            {
                PersistedData.Remove(key);
            }
            return this;
        }

        internal DataPersister Save()
        {
            if (!FileAccess.FileExists(filePath))
            {
                InitiateFirstData();
            }
            using FileAccess fileAccess = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
            fileAccess.StoreString(JsonConvert.SerializeObject(PersistedData));
            //file.StorePascalString(JsonConvert.SerializeObject(PersistedData));
            fileAccess.Close();
            return this;
        }

        internal DataPersister AddDataAndSave(string key, object value)
        {
            return AddData(key, value).Save();
        }

        internal DataPersister RemoveDataAndSave(string key)
        {
            return RemoveData(key).Save();
        }

        internal DataPersister Load()
        {
            if (!FileAccess.FileExists(filePath))
            {
                InitiateFirstData();
            }
            using FileAccess fileAccess = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileAccess.GetAsText());
            //Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(file.GetPascalString());
            fileAccess.Close();
            PersistedData = data;
            return this;
        }

        internal T GetValue<T>(string key)
        {
            if (PersistedData.ContainsKey(key))
            {
                if(PersistedData[key] is string a){
                    T result = JsonConvert.DeserializeObject<T>(a);
                    return result;
                }
                Logger.LogErr(string.Format("The value of {0} is not string.", key));
            }
            Logger.Log("Can not retrieve the value of ", key, " from data cache.");
            return default;
        }

        internal bool TryGetValue<T>(string key, out T value)
        {
            if (PersistedData.ContainsKey(key))
            {
                try
                {
                    T result = JsonConvert.DeserializeObject<T>(PersistedData[key].ToString());
                    value = result;
                    return true;
                }
                catch (System.Exception e)
                {
                    Logger.LogErr("Failed to deserialize key of ", key);
                    Logger.LogErr(e.Message);
                    value = default;
                    return false;
                }
            }
            value = default;
            return false;
        }

        private void InitiateFirstData()
        {
            using DirAccess dirAccess = DirAccess.Open(folderPath);
            if (!dirAccess.DirExists(folderPath))
            {
                dirAccess.MakeDir(folderPath);
            }

            using FileAccess fileAccess = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);
            fileAccess.StoreString(JsonConvert.SerializeObject(PersistedData));
            fileAccess.Close();
        }
    }
}
