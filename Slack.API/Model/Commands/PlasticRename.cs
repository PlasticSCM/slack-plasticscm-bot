using Mono.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slack.API.Model.Commands
{
    public class PlasticRename : BaseCommand
    {
        string attribute;
        string branch;
        string label;
        string repository;

        string newName;

        public PlasticRename(string requestedCommand)
        {
            string[] args = ExtractArgs(requestedCommand);
            new OptionSet()
            {
                { "att|attribute=", (string v) => attribute = v },
                { "br|branch=", (string v) => branch = v },
                { "lb|label=", (string v) => label = v },
                { "rep|repository=", (string v) => repository = v },
                { "to=", (string v) => newName = v }
            }.Parse(args);
        }
    }
}
