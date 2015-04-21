using Mono.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slack.API.Model.Commands
{
    public class PlasticLatest : BaseCommand
    {
        bool branches;
        bool changesets;
        string requestedCommand;

        public PlasticLatest(string requestedCommand)
        {
            string[] args = ExtractArgs(requestedCommand);
            this.requestedCommand = requestedCommand;
            new OptionSet() {
                { "branches", v => branches = (v != null) },
                { "changesets", v => changesets = (v != null) }
            }.Parse(args);
        }

        public bool BranchesRequested()
        {
            return branches;
        }

        public bool ChangesetsRequested()
        {
            return changesets;
        }
    }
}
