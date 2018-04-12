using Newtonsoft.Json;
using System.Collections.Generic;

namespace SeleniumExtras.Environment
{
    [JsonObject]
    class TestEnvironment
    {
        [JsonProperty]
        public string DriverServiceLocation { get; set; }

        [JsonProperty]
        public string ActiveDriverConfig { get; set; }

        [JsonProperty]
        public string ActiveWebsiteConfig { get; set; }

        [JsonProperty]
        public Dictionary<string, WebsiteConfig> WebSiteConfigs { get; set; }

        [JsonProperty]
        public Dictionary<string, DriverConfig> DriverConfigs { get; set; }
    }
}
