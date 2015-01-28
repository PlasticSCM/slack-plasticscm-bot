using Newtonsoft.Json;

namespace SlackBot
{
    public class StandardResponse
    {
        [JsonProperty("ok")]
        public bool ok { get; set; }
    }
}
