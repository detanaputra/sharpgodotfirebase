using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SharpGodotFirebase.Firestore
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class FirestoreError
    {
        public ErrorAttribute Error { get; set; }

        internal static FirestoreError GenerateError(int code = 2, string message = "Can not fetch document")
        {
            return new FirestoreError()
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
