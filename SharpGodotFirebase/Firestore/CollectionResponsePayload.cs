using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System.Collections.Generic;

namespace SharpGodotFirebase.Firestore
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    internal class CollectionResponsePayload
    {
        public IEnumerable<DocumentResponsePayload> Documents { get; set; }
    }
}