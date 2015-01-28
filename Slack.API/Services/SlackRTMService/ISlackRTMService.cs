namespace SlackBot.Portable.Services
{
    using SlackBot.Portable.Model;
    using System;

    public interface ISlackRTMService
    {
        event EventHandler<SlackEventArgs> SlackDataReceived;

        void ConnectPlasticSlackBot(string botToken);
    }
}
