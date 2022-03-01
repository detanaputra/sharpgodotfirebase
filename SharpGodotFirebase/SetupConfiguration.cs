using Godot;

namespace SharpGodotFirebase
{
    /// <summary>
    /// Configuration class needed for initializing SharpGodotFirebase.
    /// </summary>
    public class SetupConfiguration
    {
        /// <summary>
        /// Parent Node for SharpGodotFirebase node. This node should be an Autoload / Singleton. <br />
        /// For more information about Godot Autoload, click <see href="https://docs.godotengine.org/en/stable/tutorials/scripting/singletons_autoload.html">here</see>
        /// </summary>
        public Node ParentNode { get; set; }
        
        /// <summary>
        /// This is your Firebase project WebApiKey. You can find it in Project Setting. <br />
        /// </summary>
        public string WebAPIKey { get; set; } = string.Empty;

        /// <summary>
        /// Configuration to set Firebase REST Url to your Firebase project or to Firebase Emulator.
        /// </summary>
        public bool UseEmulator { get; set; } = false;

        /// <summary>
        /// Your Firebase Project Id
        /// </summary>
        public string ProjectId { get; set; } = string.Empty;

        /// <summary>
        /// Your Firebase AppId
        /// </summary>
        public string AppId { get; set; } = string.Empty;

        /// <summary>
        /// Your OAuth 2.0 ClientId. You can find it in your project at Google Cloud Platform in OAuth 2.0 Client IDs.
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// Your OAuth 2.0 ClientId's Secret. You can find it in your project at Google Cloud Platform in OAuth 2.0 Client IDs.
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;

        /// <summary>
        /// Firebase emulator port for Authentication process. The default is 9099.
        /// </summary>
        public string AuthEmulatorPort { get; set; } = "9099";

        /// <summary>
        /// Firebase emulator port for Firestore process. The default is 8080.
        /// </summary>
        public string FirestoreEmulatorPort { get; set; } = "8080";

        /// <summary>
        /// Firebase emulator port for RealtimeDB process. The default is 9000.
        /// </summary>
        public string RealtimeDBEmultorport { get; set; } = "9000";

        /// <summary>
        /// Your RealtimeDB region. for example: "asia-southeast1".
        /// </summary>
        public string RealtimeDBRegion { get; set; }

        /// <summary>
        /// An API SECRET generated in the Google Analytics UI. To create a new secret, navigate to: <br />
        /// Admin > Data Streams > choose your stream > Measurement Protocol > Create. See <see href="https://developers.google.com/analytics/devguides/collection/protocol/ga4/sending-events?client_type=firebase#required_parameters">here </see>
        /// </summary>
        public string AnalyticApiSecret { get; set; } = string.Empty;

        public string MeasurementId { get; set; } = string.Empty;

        public SetupConfiguration()
        {

        }
    }
}
