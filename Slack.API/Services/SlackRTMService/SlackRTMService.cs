namespace SlackBot.Portable.Services
{
    using Codice.CmdRunner;
using Newtonsoft.Json;
using Slack.API.Services;
using SlackBot.Portable.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

    public class SlackRTMService : ISlackRTMService
    {
        private const string RtmStart = "https://slack.com/api/rtm.start?token={0}";
        private const string InvalidMessage =
            "Could you ask again? I didn't understand you :(";
        private const string WaitMessage = @"As you wish, my lord.
Please wait while I gather the required information.
You can take a look at thiz purfffect picturez meanwhile... 
http://bit.ly/cuteSCMkitty";
        private const int TimerInterval = 25000;
        private const string JsonOpenResponse = @"{{""id"" : {0}, ""type"" : ""ping""}}";

        private readonly IPlasticCMDService plasticService;

        private Timer timer;

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
            SendMessage(socket, messageReceived, WaitMessage);

            string cmdResult = QueryServer(messageReceived.Text);

            SendMessage(socket, messageReceived, TrimResponse(cmdResult));
        }

        private string QueryServer(string requestedCommand)
        {
            if (requestedCommand.ToLowerInvariant().Contains("latest branches"))
                return this.plasticService.GetLatestBranches();

            if (requestedCommand.ToLowerInvariant().Contains("latest changesets"))
                return this.plasticService.GetLatestChangesets();

            return InvalidMessage;
        }

        private string TrimResponse(string response)
        {
            if (response.Length <= 800)
                return response;
            string result = response.Substring(response.Length - 796, 796);
            result = result.Substring(result.IndexOf("\n"));
            return result;
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
