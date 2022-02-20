using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SharpGodotFirebase.Authentications
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal class RefreshTokenResponsePayload
    {
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
        public string IdToken { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
    }
}
