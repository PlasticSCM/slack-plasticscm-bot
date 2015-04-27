using Mono.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Example of use:
//
// plastic switch repository to=documentacion
namespace Slack.API.Model.Commands
{
    class PlasticSwitch : BaseCommand
    {
        private bool toLabel;
        private bool toChangeset;
        private bool toBranch;
        private bool toRepository;
        private bool help;
        private string destination;

        private static string helpStr =
@"`plastic switch [rep | repository] to=repname` (see `plastic list rep`)
`plastic switch [h | help]`";

        public PlasticSwitch(string requestedCommand)
        {
            string[] args = ExtractArgs(requestedCommand);
            new OptionSet()
            {
                { "lb|label", v => toLabel = (v != null) },
                { "cs|changeset", v => toChangeset = (v != null) },
                { "br|branch", v => toBranch = (v != null) },
                { "rep|repository", v => toRepository = (v != null) },
                { "h|help", v => help = (v != null) },
                { "to=", (string v) => destination = v }
            }.Parse(args);
        }

        public bool IsToLabel()
        {
            return toLabel;
        }

        public bool IsToChangeset()
        {
            return toChangeset;
        }

        public bool IsToBranch()
        {
            return toBranch;
        }

        public bool IsToRepository()
        {
            return toRepository;
        }

        public bool IsHelp()
        {
            return help;
        }

        public static string GetHelp()
        {
            return helpStr;
        }

        public string GetDestination()
        {
            return destination;
        }
    }
}
