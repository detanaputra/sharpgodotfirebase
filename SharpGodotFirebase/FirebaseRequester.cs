using Godot;

using SharpGodotFirebase.Utilities;

using SignalStringProvider;

using System.Threading.Tasks;

namespace SharpGodotFirebase
{
    internal abstract class FirebaseRequester : Node
    {
        protected static HttpRequest HttpRequest;
        
        protected virtual async Task<IRequestResult> SendRequest(HttpRequest httpRequest, string address, string content = "", string[] customHeader = null, HttpClient.Method method = HttpClient.Method.Post)
        {
            Error err = httpRequest.Request(address, customHeader, method, content);
            if (err != Error.Ok)
            {
                Logger.LogErr("http request can not be made");
                return new RequestResult()
                {
                    Result = 4,
                    ResponseCode = -1,
                };
            }
            Variant[] signalResult = await ToSignal(httpRequest, HttpRequest.SignalName.RequestCompleted);

            return new RequestResult()
            {
                Result = (int)signalResult[0],
                ResponseCode = (int)signalResult[1],
                Header = (string[])signalResult[2],
                Body = ((byte[])signalResult[3]).GetStringFromUtf8()
            };
        }

    }
}
