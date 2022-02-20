using Godot;

using Newtonsoft.Json;

using SharpGodotFirebase.Utilities;

using SignalStringProvider;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharpGodotFirebase.Firestore
{
    internal class FirestoreDB : FirebaseRequester
    {
        private static FirestoreDB firestoreDBNode;

        internal static void Initialize(HTTPRequest hTTPRequest)
        {
            HttpRequest = hTTPRequest;
            firestoreDBNode = new FirestoreDB();
            FirebaseClient.Config.ParentNode.AddChild(firestoreDBNode);
        }

        /*internal async Task<List<T>> GetCollection<T>(CollectionReference collectionReference)
        {
            
        }*/

        internal async Task<FirestoreResult<T>> GetDocument<T>(DocumentReference documentReference)
        {
            string address = UrlBuilder.GetFirestoreUrl(documentReference.Path);
            IRequestResult requestResult = await SendRequest(HttpRequest, address, "", null, HTTPClient.Method.Get);
            FirestoreResult<T> firestoreResult = new FirestoreResult<T>(requestResult);
            if (firestoreResult.EnsureSuccess())
            {
                Logger.Log("Document received");
                GD.Print(requestResult.Body);
                DocumentResponsePayload payload = JsonConvert.DeserializeObject <DocumentResponsePayload>(requestResult.Body);
                //firestoreResult.Data = payload.Fields;
            }
            else
            {
                if (string.IsNullOrEmpty(firestoreResult.Body))
                {
                    firestoreResult.FirestoreError = FirestoreError.GenerateError();
                }
                else
                {
                    firestoreResult.FirestoreError = JsonConvert.DeserializeObject<FirestoreError>(firestoreResult.Body);
                }
                Logger.LogErr(firestoreResult.FirestoreError.Error.Code, firestoreResult.FirestoreError.Error.Message);
            }
            return firestoreResult;
        }

        protected override Task<IRequestResult> SendRequest(HTTPRequest httpRequest, string address, string content = "", string[] header = null, HTTPClient.Method method = HTTPClient.Method.Post)
        {
            string[] customHeader = header;
            if (customHeader == null)
            {
                customHeader = new string[3]
                {
                    "Content-Type: application/json",
                    "Accept: application/json",
                    "Authorization : Bearer [ID_TOKEN]"
                };
            }

            customHeader[2] = customHeader[2].Replace("[ID_TOKEN]", FirebaseClient.User.IdToken);
            return base.SendRequest(httpRequest, address, content, customHeader, method);
        }

    }
}
