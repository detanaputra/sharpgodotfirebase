using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SharpGodotFirebase.Authentications
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class FirebaseUser
    {
        public string Kind { get; set; } = string.Empty;
        public string LocalId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string IdToken { get; set; } = string.Empty;
        public bool Registered { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
