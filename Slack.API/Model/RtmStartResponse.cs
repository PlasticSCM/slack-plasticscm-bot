using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
