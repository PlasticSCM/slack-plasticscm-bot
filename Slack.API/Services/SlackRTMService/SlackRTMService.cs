namespace SlackBot.Portable.Services
{
    using Newtonsoft.Json;
    using Slack.API.Services;
    using SlackBot.Portable.Model;
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;
    using WebSocketSharp;

    public class SlackRTMService : ISlackRTMService
    {
        private const string RtmStart = "https://slack.com/api/rtm.start?token={0}";
        private const int TimerInterval = 25000;
        private const int MaxLength = 800;
        private const int OverflowInterval = 10000000;
        private const string JsonOpenResponse = @"{{""id"" : {0}, ""type"" : ""ping""}}";
        private const string LatestBranches = "latest branches";
        private const string LatestChangesets = "latest changesets";
        private const string InvalidMessage =
            @"Could you ask again? I didn't understand you :pensive:
These are the commands I understand:
> `plastic latest changesets`
> `plastic latest branches`";
        private const string WaitMessage = @"As you wish, my lord.
Please wait while I gather the required information.
I assure you, this will only take a moment. :suspect:";
        private const string OverflowMessage =
            "*Whoa, whoa!* :worried: What are you doing? Give me some space here, will you? :hurtrealbad:";

        private readonly IPlasticCMDService plasticService;

        private Timer timer;
        private long mLastCommandTimestamp = DateTime.Now.Ticks;

        public event EventHandler<SlackEventArgs> SlackDataReceived;

        public SlackRTMService(IPlasticCMDService plasticService)
        {
            this.plasticService = plasticService;
        }

        public void ConnectPlasticSlackBot(string botToken)
        {
            HttpWebRequest rtmStartRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(string.Format(RtmStart, botToken)));
            var rtmResult = rtmStartRequest.GetResponse();
            RtmStartResponse rtmStartResponse = null;

            using (var stream = new StreamReader(rtmResult.GetResponseStream()))
            {
                string responseString = stream.ReadToEnd();

                if (string.IsNullOrEmpty(responseString))
                    return;

                rtmStartResponse = JsonConvert.DeserializeObject<RtmStartResponse>(responseString);
            }
            ConnectWebSocket(rtmStartResponse);
        }

        private void ConnectWebSocket(RtmStartResponse rtmStartResponse)
        {
            if (rtmStartResponse == null || string.IsNullOrEmpty(rtmStartResponse.RtmUrl))
                return;

            WebSocketSharp.WebSocket socket = new WebSocketSharp.WebSocket(rtmStartResponse.RtmUrl);
            socket.OnMessage += socket_OnMessage;
            socket.OnOpen += socket_OnOpen;
            socket.Connect();
        }

        void socket_OnOpen(object sender, EventArgs e)
        {
            var socket = (WebSocket)sender;
            this.timer = new Timer((state) =>
            {
                socket.Send(string.Format(JsonOpenResponse, DateTime.Now.Ticks));
            }, null, 0, TimerInterval);
        }

        void socket_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            SlackMessage receivedResponse;

            try
            {
                receivedResponse = JsonConvert.DeserializeObject<SlackMessage>(e.Data);

                //Then continue processing the message...
                if (IsInvalidResponse(receivedResponse.Text))
                    return;

                ProcessMessageForPlastic((WebSocket)sender, receivedResponse);
            }
            catch (Exception ex)
            {
                receivedResponse = CreateBadJsonSlackMessage(ex);
            }

            //Show we have a new message...
            if (SlackDataReceived != null)
            {
                SlackDataReceived(this, new SlackEventArgs(receivedResponse));
            }
        }

        SlackMessage CreateBadJsonSlackMessage(Exception ex)
        {
            return new SlackMessage()
            {
                Text = ex.Message
            };
        }

        bool IsInvalidResponse(string receivedResponse)
        {
            return string.IsNullOrEmpty(receivedResponse)
                || receivedResponse == "pong"
                || !receivedResponse.ToLowerInvariant().StartsWith("plastic");
        }

        private void ProcessMessageForPlastic(WebSocket socket, SlackMessage messageReceived)
        {
            if ((DateTime.Now.Ticks - mLastCommandTimestamp) < OverflowInterval)
            {
                SendMessage(socket, messageReceived, OverflowMessage);
                return;
            }

            if (!IsValidCommand(messageReceived.Text))
            {
                SendMessage(socket, messageReceived, InvalidMessage);
                return;
            }

            SendMessage(socket, messageReceived, WaitMessage);

            string queryResult = QueryServer(messageReceived.Text);

            SendMessage(socket, messageReceived, TrimResponse(queryResult));

            mLastCommandTimestamp = DateTime.Now.Ticks;
        }

        private bool IsValidCommand(string requestedCommand)
        {
            return requestedCommand.ToLowerInvariant().Contains(LatestBranches)
                || requestedCommand.ToLowerInvariant().Contains(LatestChangesets);
        }

        private string QueryServer(string requestedCommand)
        {
            if (requestedCommand.ToLowerInvariant().Contains(LatestBranches))
                return this.plasticService.GetLatestBranches();

            if (requestedCommand.ToLowerInvariant().Contains(LatestChangesets))
                return this.plasticService.GetLatestChangesets();

            return InvalidMessage;
        }

        private string TrimResponse(string response)
        {
            if (response.Length <= MaxLength)
                return response;

            string firstLine = response.Substring(0, response.IndexOf('\n') + 1);
            int bodyLength = MaxLength - firstLine.Length;
            string body = response.Substring(response.Length - bodyLength, bodyLength);
            body = body.Substring(body.IndexOf("\n"));
            return firstLine + body;
        }

        private static void SendMessage(WebSocket socket, SlackMessage messageReceived, string cmdResult)
        {
            SlackMessage response = new SlackMessage()
            {
                Id = DateTime.Now.Ticks.ToString(),
                Channel = messageReceived.Channel,
                User = messageReceived.User,
                Type = "message",
                Text = cmdResult
            };

            string msg = JsonConvert.SerializeObject(response);
            socket.Send(msg);
        }
    }
}
