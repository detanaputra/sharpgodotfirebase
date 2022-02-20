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

        protected static Dictionary<string, object> PersistedData { get; set; } = new Dictionary<string, object>();

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

        internal DataPersister Save()
        {
            File file = new File();
            if (!file.FileExists(filePath))
            {
                InitiateFirstData();
            }
            file.Open(filePath, File.ModeFlags.Write);
            file.StoreString(JsonConvert.SerializeObject(PersistedData));
            file.Close();
            return this;
        }

        internal DataPersister AddDataAndSave(string key, object value)
        {
            AddData(key, value);
            Save();
            return this;
        }

        internal DataPersister Load()
        {
            File file = new File();
            if (!file.FileExists(filePath))
            {
                InitiateFirstData();
            }
            file.Open(filePath, File.ModeFlags.Read);
            Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(file.GetAsText());
            file.Close();
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
            Logger.LogErr("Can not retrieve the value of ", key, ". If you never invoke any signin before, then it's Okay.");
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
            Directory directory = new Directory();
            if (!directory.DirExists(folderPath))
            {
                directory.MakeDir(folderPath);
            }
            File file = new File();
            file.Open(filePath, File.ModeFlags.Write);
            file.StoreString(JsonConvert.SerializeObject(PersistedData));
            file.Close();
        }
    }
}
