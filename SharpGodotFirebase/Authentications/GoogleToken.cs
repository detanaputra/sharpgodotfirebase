using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SharpGodotFirebase.Authentications
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal class GoogleToken
    {
        public string AccessToken { get; set; }
        public string ExpiresIn { get; set; }
        public string Scope { get; set; }
        public string TokenType { get; set; }
        public string IdToken { get; set; }
    }
}
