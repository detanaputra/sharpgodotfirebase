namespace SharpGodotFirebase
{
    public interface IRequestResult
    {
        int Result { get; set; }
        int ResponseCode { get; set; }
        string[] Header { get; set; }
        string Body { get; set; }

        bool EnsureSuccess();
    }
}