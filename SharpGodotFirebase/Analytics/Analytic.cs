using Godot;

using Newtonsoft.Json;

using SharpGodotFirebase.Utilities;

using System.Threading.Tasks;

namespace SharpGodotFirebase.Analytics
{
    internal class Analytic : FirebaseRequester
    {
        private static Analytic analyticNode;

        internal static void Initialize(HttpRequest hTTPRequest)
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
            
            if(FirebaseClient.User == null)
            {
                return new RequestResult()
                {
                    Result = -1,
                    ResponseCode = -1,
                    Body = "Can not send event. Reason: User is signed out."
                };
            }

            var body = new
            {
                app_instance_id = appInstanceId,
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
            if (FirebaseClient.User == null)
            {
                return new RequestResult()
                {
                    Result = -1,
                    ResponseCode = -1,
                    Body = "Can not send event. Reason: User is signed out."
                };
            }
            var body = new
            {
                app_instance_id = appInstanceId,
                user_id = FirebaseClient.User.LocalId,
                events
            };
            string content = JsonConvert.SerializeObject(body);
            IRequestResult requestResult = await SendRequest(HttpRequest, address, content);
            return requestResult;
        }

        protected override Task<IRequestResult> SendRequest(HttpRequest httpRequest, string address, string content = "", string[] customHeader = null, HttpClient.Method method = HttpClient.Method.Post)
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
