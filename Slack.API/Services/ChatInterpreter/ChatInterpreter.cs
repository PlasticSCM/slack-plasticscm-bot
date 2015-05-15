using System.Text;

using Slack.API.Model.Commands;

namespace Slack.API.Services
{
    public class ChatInterpreter : IChatInterpreter
    {
        public ChatInterpreter(IPlasticCMDService plasticService)
        {
            this.plasticService = plasticService;
        }

        public bool IsValidCommand(string requestedCommand)
        {
            return requestedCommand.Contains(Latest)
                || requestedCommand.Contains(Switch)
                || requestedCommand.Contains(List)
                || requestedCommand.Contains(Branchhistory)
                || requestedCommand.Contains(Hal);
        }

        public string GetInvalidMessageResponse()
        {
            return InvalidMessage;
        }

        public string ProcessCommand(string requestedCommand)
        {
            string result = null;
            if (requestedCommand.StartsWith("plastic latest"))
                result = ProcessPlasticLatest(requestedCommand);

            if (requestedCommand.StartsWith("plastic li"))
                result = plasticService.GetLicenseInfo();

            if (requestedCommand.StartsWith("plastic switch"))
                result = ProcessPlasticSwitch(requestedCommand);

            if (requestedCommand.StartsWith("plastic list"))
                result = ProcessPlasticList(requestedCommand);

            if (requestedCommand.StartsWith("plastic branchhistory"))
                result = ProcessBranchHistory(requestedCommand);

            if (requestedCommand.StartsWith("plastic hal"))
                result = ProcessReleasebuilderStatus(requestedCommand);

            return result == null ? InvalidMessage : result;
        }

        string ProcessPlasticLatest(string requestedCommand)
        {
            PlasticLatest command = new PlasticLatest(requestedCommand);
            StringBuilder result = new StringBuilder();
            if (command.BranchesRequested())
                result.Append(plasticService.GetLatestBranches());

            if (command.ChangesetsRequested())
            {
                if (!command.ChangesetsFromBranchRequested())
                    result.Append(plasticService.GetLatestChangesets());
                else
                {
                    string requestedBranch = command.GetRequestedBranch();
                    result.Append(plasticService.GetLatestChangesetsFromBranch(requestedBranch));
                }
            }

            if (command.MergesRequested())
            {
                if (command.SourceBranchRequested())
                    result.Append(plasticService.FindMergeFromSrc(command.GetRequestedSrcBranch()));

                if (command.DestinationBranchRequested())
                    result.Append(plasticService.FindMergeFromDst(command.GetRequestedDstBranch()));
            }

            if (command.HelpRequested())
                result.Append(PlasticLatest.GetHelp());
                
            if (result.Length != 0)
                return result.ToString();
            return PlasticLatest.GetHelp();
        }

        string ProcessPlasticSwitch(string requestedCommand)
        {
            PlasticSwitch command = new PlasticSwitch(requestedCommand);
            StringBuilder result = new StringBuilder();
            if (command.IsToRepository())
                result.Append(plasticService.SwitchToRepository(command.GetDestination()));

            if (command.IsHelp())
                result.Append(PlasticSwitch.GetHelp());

            if (result.Length != 0)
                return result.ToString();
            return PlasticSwitch.GetHelp();
        }

        string ProcessPlasticList(string requestedCommand)
        {
            PlasticList command = new PlasticList(requestedCommand);
            StringBuilder result = new StringBuilder();
            if (command.RepositoriesRequested())
                result.Append(plasticService.ListRepositories());

            if (command.LabelsRequested())
            {
                if (!command.LabelsInBranchRequested())
                    result.Append(plasticService.ListLabels());

                else
                    result.Append(plasticService.ListLabelsInBranch(command.BranchRequested()));
            }
                

            if (command.HelpRequested())
                result.Append(PlasticList.GetHelp());

            if (result.Length != 0)
                return result.ToString();
            return PlasticList.GetHelp();
        }

        string ProcessBranchHistory(string requestedCommand)
        {
            PlasticBranchHistory command = new PlasticBranchHistory(requestedCommand);
            string branch = command.GetBranch();
            string repository = command.GetRepository();

            if (branch == null || branch.Length == 0 || command.IsHelp())
                return PlasticBranchHistory.GetHelp();

            if (command.IsSpecificRepository())
                return plasticService.BranchHistory(branch, repository);

            return plasticService.BranchHistory(branch);
        }

        string ProcessReleasebuilderStatus(string requestedCommand)
        {
            PlasticHal command = new PlasticHal(requestedCommand);
            StringBuilder result = new StringBuilder();

            if (command.StatusRequested())
                result.Append(plasticService.HalStatus());

            if (command.HelpRequested())
                result.Append(PlasticHal.GetHelp());

            if (result.Length != 0)
                return result.ToString();
            return PlasticHal.GetHelp();
        }

        const string Latest = "latest";
        const string Switch = "switch";
        const string List = "list";
        const string Branchhistory = "branchhistory";
        const string Hal = "hal";
        const string InvalidMessage =
            @"Could you ask again? I didn't understand you :pensive:
These are the commands I understand (type command name + ""h"" or ""help"" to get, well, you know, help.):
> `plastic latest`
> `plastic switch`
> `plastic list`
> `plastic branchhistory`
> `plastic hal`";
        readonly IPlasticCMDService plasticService;

    }
}
