﻿using Newtonsoft.Json;

namespace SlackBot
{
    public class AuthTestResponse
    {
        [JsonProperty("ok")]
        public bool ok { get; set; }

        [JsonProperty("url")]
        public string url { get; set; }

        [JsonProperty("team")]
        public string team { get; set; }

        [JsonProperty("user")]
        public string user { get; set; }

        [JsonProperty("team_id")]
        public string team_id { get; set; }

        [JsonProperty("okuser_id")]
        public string user_id { get; set; }
    }
}
