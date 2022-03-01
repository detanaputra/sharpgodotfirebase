using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SharpGodotFirebase.Firestore
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class DocumentResponsePayload
    {
        public string Name { get; set; }
        public string Fields { get; set; }
        public string CreatedTime { get; set; }
        public string UpdatedTime { get; set; }
    }
}
