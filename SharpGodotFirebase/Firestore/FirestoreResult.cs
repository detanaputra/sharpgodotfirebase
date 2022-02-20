using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SharpGodotFirebase.Firestore
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public sealed class FirestoreResult<T> : IRequestResult
    {
        public T Data { get; set; }
        public FirestoreError FirestoreError { get; set; }

        public FirestoreResult()
        {

        }

        public FirestoreResult(IRequestResult requestResult)
        {
            Result = requestResult.Result;
            ResponseCode = requestResult.ResponseCode;
            Header = requestResult.Header;
            Body = requestResult.Body;

            // parse body here then assign to Data
        }

        public int Result { get; set; }
        public int ResponseCode { get; set; }
        public string[] Header { get; set; }
        public string Body { get; set; }

        public bool EnsureSuccess()
        {
            if (Result == 0 & ResponseCode == 200)
            {
                return true;
            }
            return false;
        }


    }
}
