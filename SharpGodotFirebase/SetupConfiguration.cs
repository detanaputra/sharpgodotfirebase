using Godot;

namespace SharpGodotFirebase
{
    public class SetupConfiguration
    {
        public Node ParentNode { get; set; }
        public string WebAPIKey { get; set; } = string.Empty;
        public bool UseEmulator { get; set; } = false;
        public string ProjectID { get; set; } = string.Empty;

        public string AppId { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;

        public string AuthEmulatorPort { get; set; } = "9099";
        public string FirestoreEmulatorPort { get; set; } = "8080";
        public string RealtimeDBEmultorport { get; set; } = "9000";

        public string RealtimeDBRegion { get; set; }

        public SetupConfiguration()
        {

        }
    }
}
