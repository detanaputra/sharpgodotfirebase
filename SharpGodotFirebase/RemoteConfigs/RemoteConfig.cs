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

        internal static void Initialize(HTTPRequest hTTPRequest)
        {
            HttpRequest = hTTPRequest;
            remoteConfigNode = new RemoteConfig() { Name = "RemoteConfig" };
            FirebaseClient.Config.ParentNode.AddChild(remoteConfigNode);
        }

        internal async Task<RemoteConfigResult> GetRemoteConfig()
        {
            string address = UrlBuilder.GetRemoteConfigUrl();
            IRequestResult requestResult = await SendRequest(HttpRequest, address, "", null, HTTPClient.Method.Get);
            RemoteConfigResult remoteConfigResult = new RemoteConfigResult(requestResult);
            if (remoteConfigResult.EnsureSuccess())
            {
                remoteConfigResult.Config = JsonConvert.DeserializeObject<Dictionary<string, object>>(remoteConfigResult.Body);
            }
            GD.Print(remoteConfigResult.Body);
            return remoteConfigResult;
        }

        protected override Task<IRequestResult> SendRequest(HTTPRequest httpRequest, string address, string content = "", string[] customHeader = null, HTTPClient.Method method = HTTPClient.Method.Post)
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
            string idToken = FirebaseClient.GetIdToken();
            header[1] = string.Format("Authorization: Bearer {0}", idToken);

            return base.SendRequest(httpRequest, address, content, header, method);
        }
    }
}
