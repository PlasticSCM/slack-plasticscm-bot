using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slack.API.Services
{
    public interface IChatInterpreter
    {
        bool IsValidCommand(string requestedCommand);

        string ProcessCommand(string requestedCommand);
    }
}
