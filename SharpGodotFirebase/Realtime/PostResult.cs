using Newtonsoft.Json;

namespace SharpGodotFirebase.Realtime
{
    [System.Obsolete("Do not use this class", true)]
    public class PostResult<T>
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        public T Data { get; set; }
    }
}
