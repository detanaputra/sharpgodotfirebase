namespace SharpGodotFirebase.Realtime
{
    public class RealtimeResult<T> : IRequestResult
    {
        public T Data { get; set; }
        public RealtimeError RealtimeError { get; set; }

        public RealtimeResult()
        {
        }

        public RealtimeResult(IRequestResult requestResult)
        {
            Result = requestResult.Result;
            ResponseCode = requestResult.ResponseCode;
            Header = requestResult.Header;
            Body = requestResult.Body;
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
