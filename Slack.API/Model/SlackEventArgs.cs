namespace SlackBot.Portable.Model
{
    using System;

    public class SlackEventArgs : EventArgs
    {
        public SlackEventArgs(SlackMessage data)
        {
            Data = data;
        }

        public SlackMessage Data { get; set; }
    }
}
