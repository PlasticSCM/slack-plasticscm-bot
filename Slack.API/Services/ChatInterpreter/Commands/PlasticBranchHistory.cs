using Mono.Options;

namespace Slack.API.Model.Commands
{
    public class PlasticBranchHistory : BaseCommand
    {
        public PlasticBranchHistory(string requestedCommand)
        {
            string[] args = ExtractArgs(requestedCommand);
            new OptionSet() {
                { "br=|branch=", (string v) => branch = v },
                { "rep=|repository=", (string v) => repository = v },
                { "h|help", v => help = (v != null) }
            }.Parse(args);
        }

        public string GetBranch()
        {
            return branch;
        }

        public string GetRepository()
        {
            return repository;
        }

        public bool IsSpecificRepository()
        {
            return repository != null || repository.Length > 0;
        }

        public bool IsHelp()
        {
            return help;
        }

        public static string GetHelp()
        {
            return helpStr;
        }

        string branch;
        string repository;
        bool help;
        static string helpStr =
            @"`plastic branchhistory [br=? | branch=?] {rep=? | repository=?}`
`plastic branchhistory help`";
    }
}
