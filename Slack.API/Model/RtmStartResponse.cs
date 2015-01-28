using Newtonsoft.Json;

namespace SlackBot
{
    public class RtmStartResponse
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("url")]
        public string RtmUrl { get; set; }
    }
}
