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
