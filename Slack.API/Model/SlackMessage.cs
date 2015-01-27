using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlackBot.Portable.Model
{
public class SlackMessage
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("channel")]
    public string Channel { get; set; }

    [JsonProperty("user")]
    public string User { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("ts")]
    public string Ts { get; set; }

    [JsonProperty("team")]
    public string Team { get; set; }
}
}
