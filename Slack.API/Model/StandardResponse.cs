using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlackBot
{
    public class StandardResponse
    {
        [JsonProperty("ok")]
        public bool ok { get; set; }
    }
}
