using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SeleniumExtras.Environment
{
    [JsonObject]
    public class DriverConfig
    {
        [JsonProperty]
        public string DriverTypeName { get; set; }

        [JsonProperty]
        public string AssemblyName { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public Browser BrowserValue { get; set; }

        [JsonProperty]
        public string RemoteCapabilities { get; set; }

        [JsonProperty]
        public bool AutoStartRemoteServer { get; set; }
    }
}
