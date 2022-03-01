using Godot;

using Newtonsoft.Json;

using SharpGodotFirebase.Utilities;

using System.Threading.Tasks;

namespace SharpGodotFirebase.Analytics
{
    internal class Analytic : FirebaseRequester
    {
        private static Analytic analyticNode;

        internal static void Initialize(HTTPRequest hTTPRequest)
        {
            HttpRequest = hTTPRequest;
            analyticNode = new Analytic() { Name = "Analytic" };
            FirebaseClient.Config.ParentNode.AddChild(analyticNode);
        }

        public Analytic()
        {

        }

        internal async Task<IRequestResult> SendEvent(params Event[] events)
        {
            string appInstanceId = "232de7e2791ea7b7302d3654ec103075";
            string address = UrlBuilder.GetAnalyticUrlMP4();
            var body = new
            {
                app_instance_id = appInstanceId, // TODO instance id??
                user_id = FirebaseClient.User.LocalId,
                events
            };
            string content = JsonConvert.SerializeObject(body);
            IRequestResult requestResult = await SendRequest(HttpRequest, address, content);
            return requestResult;
        }

        internal async Task<IRequestResult> SendDebugEvent(params Event[] events)
        {
            string appInstanceId = "232de7e2791ea7b7302d3654ec103075";
            string address = UrlBuilder.GetDebugAnalyticUrlMP4();
            var body = new
            {
                app_instance_id = appInstanceId,  // TODO instance id??
                user_id = FirebaseClient.User.LocalId,
                events
            };
            string content = JsonConvert.SerializeObject(body);
            IRequestResult requestResult = await SendRequest(HttpRequest, address, content);
            return requestResult;
        }

        protected override Task<IRequestResult> SendRequest(HTTPRequest httpRequest, string address, string content = "", string[] customHeader = null, HTTPClient.Method method = HTTPClient.Method.Post)
        {
            string[] header = customHeader;
            if (header == null)
            {
                /*header = new string[3]
                {
                    "Content-Type: application/json",
                    "Accept: application/json",
                    "Content-length: 0"
                };*/
                header = new string[2]
                {
                    "Content-Type: application/json",
                    "Accept: application/json"
                };
            }
            return base.SendRequest(httpRequest, address, content, header, method);
        }
    }
}
