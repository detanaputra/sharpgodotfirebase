using Godot;

using Newtonsoft.Json;

using SharpGodotFirebase.Utilities;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharpGodotFirebase.RemoteConfigs
{
    internal class RemoteConfig : FirebaseRequester
    {
        private static RemoteConfig remoteConfigNode;

        internal static void Initialize(HttpRequest hTTPRequest)
        {
            HttpRequest = hTTPRequest;
            remoteConfigNode = new RemoteConfig() { Name = "RemoteConfig" };
            FirebaseClient.Config.ParentNode.AddChild(remoteConfigNode);
        }

        internal async Task<RemoteConfigResult> GetRemoteConfig()
        {
            string address = UrlBuilder.GetRemoteConfigUrl();
            IRequestResult requestResult = await SendRequest(HttpRequest, address, "", null, HttpClient.Method.Get);
            RemoteConfigResult remoteConfigResult = new RemoteConfigResult(requestResult);
            if (remoteConfigResult.EnsureSuccess())
            {
                remoteConfigResult.Config = JsonConvert.DeserializeObject<Dictionary<string, object>>(remoteConfigResult.Body);
            }
            GD.Print(remoteConfigResult.Body);
            return remoteConfigResult;
        }

        protected override async Task<IRequestResult> SendRequest(HttpRequest httpRequest, string address, string content = "", string[] customHeader = null, HttpClient.Method method = HttpClient.Method.Post)
        {
            string[] header = customHeader;
            if (header == null)
            {
                header = new string[2]
                {
                    "Accept: application/json",
                    "[auth bearer placeholder]"
                };
            }
            string idToken = await FirebaseClient.GetIdToken();
            header[1] = string.Format("Authorization: Bearer {0}", idToken);

            return await base.SendRequest(httpRequest, address, content, header, method);
        }
    }
}
