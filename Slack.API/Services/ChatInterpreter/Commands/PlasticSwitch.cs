using Mono.Options;

namespace Slack.API.Model.Commands
{
    class PlasticSwitch : BaseCommand
    {
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

        bool toLabel;
        bool toChangeset;
        bool toBranch;
        bool toRepository;
        bool help;
        string destination;

        static string helpStr =
@"`plastic switch [rep | repository] to=repname` (see `plastic list rep`)
`plastic switch [h | help]`";

    }
}
