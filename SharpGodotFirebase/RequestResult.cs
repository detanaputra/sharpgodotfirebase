namespace SharpGodotFirebase
{
    public class RequestResult : IRequestResult
    {
        public int Result { get; set; }
        public int ResponseCode { get; set; }
        public string[] Header { get; set; }
        public string Body { get; set; }

        public RequestResult()
        {

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
