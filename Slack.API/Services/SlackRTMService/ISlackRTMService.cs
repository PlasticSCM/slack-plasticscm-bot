namespace SlackBot.Portable.Services
{
    using SlackBot.Portable.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISlackRTMService
    {
        event EventHandler<SlackEventArgs> SlackDataReceived;

        void ConnectPlasticSlackBot(string botToken);
    }
}
