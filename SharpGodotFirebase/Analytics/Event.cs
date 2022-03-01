using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using System;
using System.Collections.Generic;

namespace SharpGodotFirebase.Analytics
{
    /// <summary>
    /// Event that will be sent into Google Analytics. There are limitation about this method, see <see href="https://developers.google.com/analytics/devguides/collection/protocol/ga4/sending-events?client_type=firebase#limitations">here</see>
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Event
    {
        public string Name { get; set; }
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>(25);

        public Event(string name, Dictionary<string, object> @params)
        {
            Name = name;
            Params = @params;
        }

        public Event(string name)
        {
            Name = name;
        }
    }
}