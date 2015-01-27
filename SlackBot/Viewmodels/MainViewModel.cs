using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SlackBot.Portable.Services;
using SlackBot.Viewmodels.Base;


namespace SlackBot.Viewmodels
{

    public class MainViewModel : VMBase
    {
        private const string BotToken = "xoxb-3421343744-ayq92tRV13G2StTqMR8GFCvJ";

        private readonly ISlackRTMService slackService;

        private string messages;

        public MainViewModel(ISlackRTMService slackService)
        {
            this.slackService = slackService;

            this.slackService.SlackDataReceived += slackService_SlackDataReceived;
            this.slackService.ConnectPlasticSlackBot(BotToken);
        }

        public string Messages
        {
            get { return this.messages; }
            set
            {
                this.messages = value;
                RaisePropertyChanged();
            }
        }

        private void slackService_SlackDataReceived(object sender, Portable.Model.SlackEventArgs e)
        {
            Messages = Messages + e.Data.Text + "\r\n";
        }
    }
}
