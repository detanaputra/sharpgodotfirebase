namespace SharpGodotFirebase.Authentications
{
    public sealed class AuthResult : IRequestResult
    {
        public int Result { get; set; }
        public int ResponseCode { get; set; }
        public string[] Header { get; set; }
        public string Body { get; set; }
        public FirebaseUser User { get; set; }
        public AuthError AuthError { get; set; }

        public AuthResult()
        {

        }

        public AuthResult(IRequestResult requestResult)
        {
            Result = requestResult.Result;
            ResponseCode = requestResult.ResponseCode;
            Header = requestResult.Header;
            Body = requestResult.Body;
        }

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
