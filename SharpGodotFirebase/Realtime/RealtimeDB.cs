using Godot;

using Newtonsoft.Json;

using SharpGodotFirebase.Utilities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpGodotFirebase.Realtime
{
    internal class RealtimeDB : FirebaseRequester
    {
        private static RealtimeDB realtimeDBNode;

        internal static void Initialize(HTTPRequest hTTPRequest)
        {
            HttpRequest = hTTPRequest;
            realtimeDBNode = new RealtimeDB();
            FirebaseClient.Config.ParentNode.AddChild(realtimeDBNode);
        }

        internal async Task<RealtimeResult<T>> GetDocument<T>(ChildQuery childQuery, string database = "default")
        {
            string address = UrlBuilder.GetRealtimeUrl(childQuery.Path, database);
            IRequestResult requestResult = await SendRequest(HttpRequest, address, "", null, HTTPClient.Method.Get);
            RealtimeResult<T> realtimeResult = new RealtimeResult<T>(requestResult);
            if (realtimeResult.EnsureSuccess())
            {
                Logger.Log("Document received: ", realtimeResult.Body);
                realtimeResult.Data = JsonConvert.DeserializeObject<T>(realtimeResult.Body);
            }
            else
            {
                if (string.IsNullOrEmpty(realtimeResult.Body) | realtimeResult.Body == "null")
                {
                    realtimeResult.RealtimeError = RealtimeError.GenerateError();
                }
                else
                {
                    realtimeResult.RealtimeError = JsonConvert.DeserializeObject<RealtimeError>(realtimeResult.Body);
                }
                Logger.LogErr(realtimeResult.RealtimeError.Error.Code, realtimeResult.RealtimeError.Error.Message);
            }
            return realtimeResult;
        }

        internal async Task<RealtimeResult<T>> PutDocument<T>(ChildQuery childQuery, T data, string database = "default")
        {
            string address = UrlBuilder.GetRealtimeUrl(childQuery.Path, database);
            string content = JsonConvert.SerializeObject(data);
            IRequestResult requestResult = await SendRequest(HttpRequest, address, content, null, HTTPClient.Method.Put);
            RealtimeResult<T> realtimeResult = new RealtimeResult<T>(requestResult);
            if (realtimeResult.EnsureSuccess())
            {
                Logger.Log("Document received");
                realtimeResult.Data = JsonConvert.DeserializeObject<T>(realtimeResult.Body);
            }
            else
            {
                if (string.IsNullOrEmpty(realtimeResult.Body) | realtimeResult.Body == "null")
                {
                    realtimeResult.RealtimeError = RealtimeError.GenerateError();
                }
                else
                {
                    realtimeResult.RealtimeError = JsonConvert.DeserializeObject<RealtimeError>(realtimeResult.Body);
                }
                Logger.LogErr(realtimeResult.RealtimeError.Error.Code, realtimeResult.RealtimeError.Error.Message);
            }
            return realtimeResult;
        }

        internal async Task<RealtimeResult<T>> PostDocument<T>(ChildQuery childQuery, T data, string database = "default")
        {
            string address = UrlBuilder.GetRealtimeUrl(childQuery.Path, database);
            string content = JsonConvert.SerializeObject(data);
            IRequestResult requestResult = await SendRequest(HttpRequest, address, content, null, HTTPClient.Method.Post);
            RealtimeResult<T> realtimeResult = new RealtimeResult<T>(requestResult);
            if (realtimeResult.EnsureSuccess())
            {
                Logger.Log("Document received");
                realtimeResult.Data = data;
            }
            else
            {
                if (string.IsNullOrEmpty(realtimeResult.Body) | realtimeResult.Body == "null")
                {
                    realtimeResult.RealtimeError = RealtimeError.GenerateError();
                }
                else
                {
                    realtimeResult.RealtimeError = JsonConvert.DeserializeObject<RealtimeError>(realtimeResult.Body);
                }
                Logger.LogErr(realtimeResult.RealtimeError.Error.Code, realtimeResult.RealtimeError.Error.Message);
            }
            return realtimeResult;
        }
    }
}
