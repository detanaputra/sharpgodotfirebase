namespace SharpGodotFirebase.Realtime
{
    public class RealtimeDbDocument<T>
    {
        public string Key { get; set; } = string.Empty;
        public T Value { get; set; } = default;

        public RealtimeDbDocument(string key, T value)
        {
            Key = key;
            Value = value;
        }


    }
}
