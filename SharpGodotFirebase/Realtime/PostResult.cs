using Newtonsoft.Json;

namespace SharpGodotFirebase.Realtime
{
    public class PostResult<T>
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        public T Data { get; set; }
    }
}
