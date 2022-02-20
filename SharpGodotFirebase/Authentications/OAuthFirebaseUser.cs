using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SharpGodotFirebase.Authentications
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public sealed class OAuthFirebaseUser : FirebaseUser
    {
        public string FederatedId { get; set; }
        public string ProviderId { get; set; }
        public bool EmailVerified { get; set; }
        public string OauthIdToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        public string RawUserInfo { get; set; }
    }
}
