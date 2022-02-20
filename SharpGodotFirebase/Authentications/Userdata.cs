using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System.Collections.Generic;

namespace SharpGodotFirebase.Authentications
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public sealed class ProviderUserInfo
    {
        public string ProviderId { get; set; }
        public string DisplayName { get; set; }
        public string PhotoUrl { get; set; }
        public string FederatedId { get; set; }
        public string Email { get; set; }
        public string RawId { get; set; }
        public string ScreenName { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public sealed class User
    {
        public string LocalId { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string DisplayName { get; set; }
        public List<ProviderUserInfo> ProviderUserInfo { get; set; }
        public string PhotoUrl { get; set; }
        public string PasswordHash { get; set; }
        public double PasswordUpdatedAt { get; set; }
        public string ValidSince { get; set; }
        public bool Disabled { get; set; }
        public string LastLoginAt { get; set; }
        public string CreatedAt { get; set; }
        public bool CustomAuth { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public sealed class Userdata
    {
        public List<User> Users { get; set; }
    }

}
