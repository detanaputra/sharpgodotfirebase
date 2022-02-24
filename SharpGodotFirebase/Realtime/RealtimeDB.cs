using Godot;

using Newtonsoft.Json;

using SharpGodotFirebase.Utilities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpGodotFirebase.Realtime
{
    internal class RealtimeDb : FirebaseRequester
    {
        private static RealtimeDb realtimeDbNode;

        internal static void Initialize(HTTPRequest hTTPRequest)
        {
            HttpRequest = hTTPRequest;
            realtimeDbNode = new RealtimeDb();
            FirebaseClient.Config.ParentNode.AddChild(realtimeDbNode);
        }

        internal async Task<RealtimeResult<T>> GetDocument<T>(ChildQuery childQuery, string database = "default")
        {
            string address = UrlBuilder.GetRealtimeUrl(childQuery.Path, database);
            IRequestResult requestResult = await SendRequest(HttpRequest, address, "", null, HTTPClient.Method.Get);
            RealtimeResult<T> realtimeResult = new RealtimeResult<T>(requestResult);
            if (realtimeResult.EnsureSuccess())
            {
                Logger.Log("Document received: ", realtimeResult.Body);
                realtimeResult.Document = new RealtimeDbDocument<T>(childQuery.LastSection, JsonConvert.DeserializeObject<T>(realtimeResult.Body));
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

        internal async Task<RealtimeResult<T>> GetCollection<T>(ChildQuery childQuery, string database = "default")
        {
            string address = UrlBuilder.GetRealtimeUrl(childQuery.Path, database);
            IRequestResult requestResult = await SendRequest(HttpRequest, address, "", null, HTTPClient.Method.Get);
            RealtimeResult<T> realtimeResult = new RealtimeResult<T>(requestResult);
            if (realtimeResult.EnsureSuccess())
            {
                Logger.Log("Document received.");
                realtimeResult.Collection = ProcessRawJSONString<T>(realtimeResult.Body);
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
                Logger.Log("Document put");
                realtimeResult.Document = new RealtimeDbDocument<T>(childQuery.LastSection, JsonConvert.DeserializeObject<T>(realtimeResult.Body));
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
                Logger.Log("Document posted");
                var definition = new { name = "" };
                var body = JsonConvert.DeserializeAnonymousType(realtimeResult.Body, definition);
                realtimeResult.Document = new RealtimeDbDocument<T>(body.name, data);
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

        internal async Task<RealtimeResult<Dictionary<string, object>>> PatchDocument(ChildQuery childQuery, Dictionary<string, object> data, string database = "default")
        {
            string address = UrlBuilder.GetRealtimeUrl(childQuery.Path, database);
            string content = JsonConvert.SerializeObject(data);
            IRequestResult requestResult = await SendRequest(HttpRequest, address, content, null, HTTPClient.Method.Patch);
            RealtimeResult<Dictionary<string, object>> realtimeResult = new RealtimeResult<Dictionary<string, object>>(requestResult);
            if (realtimeResult.EnsureSuccess())
            {
                realtimeResult.Document = new RealtimeDbDocument<Dictionary<string, object>>(childQuery.LastSection, data);
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

        internal async Task<RealtimeResult> DeleteDocument(ChildQuery childQuery, string database = "default")
        {
            string address = UrlBuilder.GetRealtimeUrl(childQuery.Path, database);
            IRequestResult requestResult = await SendRequest(HttpRequest, address, "", null, HTTPClient.Method.Delete);
            RealtimeResult realtimeResult = new RealtimeResult(requestResult);
            if (realtimeResult.EnsureSuccess())
            {
                Logger.Log("Document Deleted");
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

        private IEnumerable<RealtimeDbDocument<T>> ProcessRawJSONString<T>(string rawString)
        {
            List<RealtimeDbDocument<T>> result = new List<RealtimeDbDocument<T>>();
            if (rawString.StartsWith("["))
            {
                // is array
                IList<T> jsonArray = JsonConvert.DeserializeObject<IList<T>>(rawString);
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    result.Add(new RealtimeDbDocument<T>(i.ToString(), jsonArray[i]));
                }
                return result;
            }
            Dictionary<string, T> jsonResult = JsonConvert.DeserializeObject<Dictionary<string, T>>(rawString);
            foreach(KeyValuePair<string, T> i in jsonResult)
            {
                result.Add(new RealtimeDbDocument<T>(i.Key, i.Value));
            }
            return result;
            // is object
        }
    }
}