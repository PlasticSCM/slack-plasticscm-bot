namespace Slack.API.Services
{
    using Codice.CmdRunner;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PlasticCMDService : IPlasticCMDService
    {
        private const string Repository = "default";
        private const string DateFormat = "MMM d 'at' HH:mm";
        private const string FindChangesetsCommand =
            @"cm find changesets on repositories '{0}' --format=" +
            @""">[`cs:{{changesetid}}@br:{{branch}}`] on _{{date}}_ by " +
            @"*{{owner}}*: {{comment}}"" --dateformat=""{1}"" --nototal";
        private const string FindBranchesCommand =
            @"cm find branch on repositories '{0}' --format="">[`br:{{name}}`] " +
            @"on _{{date}}_ by *{{owner}}*: {{comment}}"" --dateformat=""{1}"" --nototal";

        public string GetLatestChangesets()
        {
            string command = string.Format(FindChangesetsCommand, Repository, DateFormat);
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(command, Environment.CurrentDirectory, false);
            return string.Format("Latest changesets on repository `{0}`: \r\n {1}", Repository, cmdResult);
        }

        public string GetLatestBranches()
        {
            string command = string.Format(FindBranchesCommand, Repository, DateFormat);
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(command, Environment.CurrentDirectory, false);
            return string.Format("Latest branches on repository `{0}`: \r\n {1}", Repository, cmdResult);
        }
    }
}
