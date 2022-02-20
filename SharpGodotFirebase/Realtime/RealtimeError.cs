using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SharpGodotFirebase.Realtime
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class RealtimeError
    {
        public ErrorAttribute Error { get; set; }

        internal static RealtimeError GenerateError(int code = 2, string message = "Can not fetch document")
        {
            return new RealtimeError()
            {
                Error = new ErrorAttribute()
                {
                    Code = code,
                    Message = message,
                    Status = "Generic Error"
                }
            };
        }
    }

    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class ErrorAttribute
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
