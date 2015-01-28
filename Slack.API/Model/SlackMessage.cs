using Newtonsoft.Json;

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

    [JsonProperty("mrkdwn")]
    public bool MarkDown { get; set; }
}
}
