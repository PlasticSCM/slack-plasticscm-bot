namespace Slack.API.Services
{
    using Codice.CmdRunner;
    using Slack.API.Util;
    using System;

    public class PlasticCMDService : IPlasticCMDService
    {
        public string GetLatestChangesets()
        {
            string command = string.Format(FindChangesetsCommand, Repository, DateFormat);
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(command, Environment.CurrentDirectory, false);
            return string.Format("Latest changesets on repository `{0}`: \r\n {1}", Repository, cmdResult);
        }

        public string GetLatestChangesetsFromBranch(string requestedBranch)
        {
            string command = string.Format(FindChangesetsOnBranchCommand, requestedBranch, Repository, DateFormat);
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(command, Environment.CurrentDirectory, false);
            Console.WriteLine(command + " : " + cmdResult);
            return string.Format("Latest changesets on `{0}@{1}`: \r\n{2}", requestedBranch, Repository, cmdResult);
        }

        public string GetLatestBranches()
        {
            string command = string.Format(FindBranchesCommand, Repository, DateFormat);
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(command, Environment.CurrentDirectory, false);
            return string.Format("Latest branches on repository `{0}`: \r\n {1}", Repository, cmdResult);
        }

        public string GetLicenseInfo()
        {
            string command = "cm li";
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(command, Environment.CurrentDirectory, false);
            return string.Format("License information: \r\n {0}", cmdResult);
        }

        public string SwitchToRepository(string requestedRepo)
        {
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(ListReposCommand, Environment.CurrentDirectory, false);
            if (!cmdResult.Contains(requestedRepo))
                return string.Format(":cold_sweat: Could you repeat? I couldn't find `{0}`", requestedRepo);
            else
            {
                Repository = requestedRepo;
                return string.Format("Switched to repository `{0}`", requestedRepo);
            }
        }

        public string FindMergeFromSrc(string requestedSrc)
        {
            string command = string.Format(FindMergeFromSrcCommand, requestedSrc, Repository, DateFormat);
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(command, Environment.CurrentDirectory, false);
            Console.WriteLine(command + " : " + cmdResult);
            return string.Format("Latest merges found with source `{0}@{1}`: \r\n{2}", requestedSrc, Repository, cmdResult);
        }

        public string FindMergeFromDst(string requestedDst)
        {
            string command = string.Format(FindMergeFromDstCommand, requestedDst, Repository, DateFormat);
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(command, Environment.CurrentDirectory, false);
            Console.WriteLine(command + " : " + cmdResult);
            return string.Format("Latest merges found with destination `{0}@{1}`: \r\n{2}", requestedDst, Repository, cmdResult);
        }

        public string ListRepositories()
        {
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(ListReposCommand, Environment.CurrentDirectory, false);
            Console.WriteLine(ListReposCommand + " : " + cmdResult);
            return string.Format("Repositories found: \r\n{0}", cmdResult);
        }

        public string ListLabels()
        {
            string command = string.Format(ListLabelsCommand, Repository, DateFormat);
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(command, Environment.CurrentDirectory, false);
            Console.WriteLine(command + " : " + cmdResult);
            return string.Format("Labels found on `{0}`: \r\n{1}", Repository, cmdResult);
        }

        public string ListLabelsInBranch(string requestedBranch)
        {
            string command = string.Format(ListLabelsInBranchCommand, requestedBranch, Repository, DateFormat);
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(command, Environment.CurrentDirectory, false);
            Console.WriteLine(command + " : " + cmdResult);
            return string.Format("Labels found on `{0}@{1}`: \r\n{2}", requestedBranch, Repository, cmdResult);
        }

        public string BranchHistory(string requestedBranch)
        {
            return BranchHistory(requestedBranch, Repository);
        }

        public string BranchHistory(string requestedBranch, string requestedRep)
        {
            string command = string.Format(BranchHistoryCommand, requestedBranch, requestedRep, DateFormat);
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(command, Environment.CurrentDirectory, false);
            Console.WriteLine(command + " : " + cmdResult);
            return string.Format("Branch history for `{0}@{1}`: \r\n{2}", requestedBranch, requestedRep, Utils.FormatBranchHistory(cmdResult));
        }

        public string HalStatus()
        {
            string command = string.Format(HalStatusCurrent, HalServer);
            string cmdResult = CmdRunner.ExecuteCommandWithStringResult(command, Environment.CurrentDirectory, false);
            Console.WriteLine(command + " : " + cmdResult);
            return Utils.FormatAsCode(cmdResult);
        }

        string HalServer = "releasebuilder";
        string Repository = "default";
        const string DateFormat = "MMM d 'at' HH:mm";
        const string FindChangesetsCommand =
            @"cm find changesets on repositories '{0}' --format=" +
            @""">[`cs:{{changesetid}}@br:{{branch}}`] on _{{date}}_ by " +
            @"*{{owner}}*: {{comment}}"" --dateformat=""{1}"" --nototal";
        const string FindChangesetsOnBranchCommand =
            @"cm find changesets where branch='{0}' on repositories '{1}' --format=" +
            @""">[`cs:{{changesetid}}@br:{{branch}}`] on _{{date}}_ by " +
            @"*{{owner}}*: {{comment}}"" --dateformat=""{2}"" --nototal";
        const string FindBranchesCommand =
            @"cm find branch on repositories '{0}' --format="">[`br:{{name}}`] " +
            @"on _{{date}}_ by *{{owner}}*: {{comment}}"" --dateformat=""{1}"" --nototal";
        const string FindMergeFromSrcCommand =
            @"cm find merge where srcbranch='{0}' on repositories '{1}' --format=" +
            @""">[`{{srcbranch}}@{{srcchangeset}}` *--->* `{{dstbranch}}@{{dstchangeset}}`] " +
            @"{{type}} on _{{date}}_ by *{{owner}}*"" --dateformat=""{2}"" --nototal";
        const string FindMergeFromDstCommand =
            @"cm find merge where dstbranch='{0}' on repositories '{1}' --format=" +
            @""">[`{{srcbranch}}@{{srcchangeset}}` *--->* `{{dstbranch}}@{{dstchangeset}}`] " +
            @"{{type}} on _{{date}}_ by *{{owner}}*"" --dateformat=""{2}"" --nototal";
        const string ListReposCommand =
            @"cm lrep --format="">`#{0} - {1}`""";
        const string ListLabelsCommand =
            @"cm find labels on repositories '{0}' --format="">[`lb:{{name}}@br:{{branch}}`] on " +
            @"_{{date}}_ by *{{owner}}*"" --nototal --dateformat=""{1}""";
        const string ListLabelsInBranchCommand =
            @"cm find labels where branch='{0}' on repositories '{1}' --format="">[`lb:{{name}}`] on " +
            @"_{{date}}_ by *{{owner}}*"" --dateformat=""{2}"" --nototal";
        const string BranchHistoryCommand =
            @"cm branchhistory br:{0}@rep:{1} --dateformat=""{2}""";
        const string HalStatusCurrent =
            @"hal {0} status current";
    }
}