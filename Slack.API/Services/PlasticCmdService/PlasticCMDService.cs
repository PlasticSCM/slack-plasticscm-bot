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

        public string GetLatestChangesets()
        {
            string command = string.Format(@"cm find changesets on repositories '{0}' --format={{date}}#{{comment}} --nototal", Repository);
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(command, Environment.CurrentDirectory, false);
            return string.Format("Latest changesets on {0} repository: \r\n {1}", Repository, cmdResult);
        }

        public string GetLatestBranches()
        {
            string command = string.Format("cm find branch on repositories '{0}' --format={{id}}#{{name}} --nototal", Repository);
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(command, Environment.CurrentDirectory, false);
            return string.Format("Latest branches on {0} repository: \r\n {1}", Repository, cmdResult);
        }
    }
}
