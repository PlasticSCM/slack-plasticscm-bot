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
        private const string LatestBranches = "latest branches";
        private const string LatestChangesets = "latest changesets";
        private const string InvalidMessage =
            @"Could you ask again? I didn't understand you :pensive:
These are the commands I understand:
> `plastic latest changesets`
> `plastic latest branches`
> `plastic li`";
        private readonly IPlasticCMDService plasticService;

        public ChatInterpreter(IPlasticCMDService plasticService)
        {
            this.plasticService = plasticService;
        }

        public bool IsValidCommand(string requestedCommand)
        {
            return true;
            return requestedCommand.ToLowerInvariant().Contains(LatestBranches)
                || requestedCommand.ToLowerInvariant().Contains(LatestChangesets);
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

            if (requestedCommand.StartsWith("plastic find merge"))
                result = ProcessPlasticFindMerge(requestedCommand);

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
                result.Append(plasticService.GetLatestChangesets());

            if (result.Length != 0)
                return result.ToString();
            return null;
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

        private string ProcessPlasticFindMerge(string requestedCommand)
        {
            PlasticFindMerge command = new PlasticFindMerge(requestedCommand);
            StringBuilder result = new StringBuilder();
            if (command.IsSrcBranch())
                result.Append(plasticService.FindMergeFromSrc(command.GetSrcBranch()));

            if (command.IsDstBranch())
                result.Append(plasticService.FindMergeFromDst(command.GetDstBranch()));

            if (command.IsHelp())
                result.Append(PlasticFindMerge.getHelp());

            if (result.Length != 0)
                return result.ToString();
            return PlasticFindMerge.getHelp();
        }

        private string ProcessPlasticList(string requestedCommand)
        {
            PlasticList command = new PlasticList(requestedCommand);
            StringBuilder result = new StringBuilder();
            if (command.IsRepositories())
                result.Append(plasticService.ListRepositories());

            if (command.IsLabels())
                result.Append(plasticService.ListLabels());

            if (command.IsHelp())
                result.Append(PlasticList.GetHelp());

            if (result.Length != 0)
                return result.ToString();
            return PlasticList.GetHelp();
        }
    }
}
