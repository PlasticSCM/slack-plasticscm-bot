using Mono.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slack.API.Model.Commands
{
    public class PlasticList : BaseCommand
    {
        private bool repositories;
        private bool labels;
        private bool help;
        private static string helpStr =
            @"`plastic list [rep | repositories]`
`plastic list [lb | labels]`
`plastic list [h | help]`";

        public PlasticList(string requestedCommand)
        {
            string[] args = ExtractArgs(requestedCommand);
            new OptionSet()
            {
                { "rep|repositories", v => repositories = (v != null) },
                { "lb|labels", v => labels = (v != null) },
                { "h|help", v => help = (v != null) }
            }.Parse(args);
        }

        public bool RepositoriesRequested()
        {
            return repositories;
        }

        public bool LabelsRequested()
        {
            return labels;
        }

        public bool HelpRequested()
        {
            return help;
        }

        public static string GetHelp()
        {
            return helpStr;
        }
    }
}
