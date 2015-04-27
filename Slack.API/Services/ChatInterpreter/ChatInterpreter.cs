using Mono.Options;
using Slack.API.Model.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slack.API.Services
{
    public class ChatInterpreter : IChatInterpreter
    {
        private const string Latest = "latest";
        private const string Switch = "switch";
        private const string List = "list";
        private const string InvalidMessage =
            @"Could you ask again? I didn't understand you :pensive:
These are the commands I understand (type command name + ""h"" or ""help"" to get, well, you know, help.):
> `plastic latest`
> `plastic switch`
> `plastic list`";
        private readonly IPlasticCMDService plasticService;

        public ChatInterpreter(IPlasticCMDService plasticService)
        {
            this.plasticService = plasticService;
        }

        public bool IsValidCommand(string requestedCommand)
        {
            return requestedCommand.Contains(Latest)
                || requestedCommand.Contains(Switch)
                || requestedCommand.Contains(List);
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

            return result == null ? InvalidMessage : result;
        }

        private string ProcessPlasticLatest(string requestedCommand)
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

        private string ProcessPlasticSwitch(string requestedCommand)
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

        private string ProcessPlasticList(string requestedCommand)
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
    }
}
