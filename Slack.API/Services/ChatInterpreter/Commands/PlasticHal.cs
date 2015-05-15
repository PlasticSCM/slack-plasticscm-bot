namespace Slack.API.Model.Commands
{
    public class PlasticHal : BaseCommand
    {
        public PlasticHal(string requestedCommand)
        {
            // Options.cs seems to be buggy.
            // Can't parse 'plastic hal' nor 'plastic hal status'
        }

        public bool StatusRequested()
        {
            return status;
        }

        public bool HelpRequested()
        {
            return help;
        }

        public static string GetHelp()
        {
            return helpStr;
        }

        bool status = true;
        bool help;
        static string helpStr =
            @"`plastic hal [status]`
`plastic hal help`";
    }
}
