using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slack.API.Model.Commands
{
    public class BaseCommand
    {
        protected string[] ExtractArgs(string requestedCommand)
        {
            string[] args = requestedCommand.Split(new char[] { ' ' });
            for (int i=0; i<args.Length; i++)
            {
                args[i] = "-" + args[i];
            }
            return args;
        }
    }
}
