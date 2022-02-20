using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System.Collections.Generic;

namespace SharpGodotFirebase.Authentications
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public sealed class AuthError
    {
        public ErrorAttribute Error { get; set; }

        internal static AuthError GenerateError(int code = 2, string message = "Connection error. Please check you internet connection.")
        {
            return new AuthError()
            {
                Error = new ErrorAttribute()
                {
                    Code = code,
                    Message = message,
                    Errors = new List<ErrorInternal>()
                    {
                        new ErrorInternal()
                        {
                            Message = "HTTPRequest node can not make connection",
                            Domain = "Internet Connection",
                            Reason = "Posible internet connection or server status"
                        }
                    }
                }
            };
        }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public sealed class ErrorAttribute
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<ErrorInternal> Errors { get; set; } = new List<ErrorInternal>();
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public sealed class ErrorInternal
    {
        public string Message { get; set; }
        public string Domain { get; set; }
        public string Reason { get; set; }
    }
}
