namespace SlackBot.Portable.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SlackEventArgs : EventArgs
    {
        public SlackEventArgs(SlackMessage data)
        {
            Data = data;
        }

        public SlackMessage Data { get; set; }
    }
}
