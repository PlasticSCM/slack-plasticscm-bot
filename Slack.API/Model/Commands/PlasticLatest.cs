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
        bool merges;
        string srcbranch;
        string dstbranch;
        string branch;
        bool help;
        static string helpStr =

@"> `plastic latest branches`
> `plastic latest changesets [br=<branch>]`
> `plastic latest merges { src=? / srcbranch=? | dst= / dstbranch=?}`
> `plastic latest [h | help]`";

        public PlasticLatest(string requestedCommand)
        {
            string[] args = ExtractArgs(requestedCommand);
            new OptionSet() {
                { "branches", v => branches = (v != null) },
                { "changesets", v => changesets = (v != null) },
                { "merges", v => merges = (v != null) },
                { "src=|srcbranch=", (string v) => srcbranch = v },
                { "dts=|dstbranch=", (string v) => dstbranch = v },
                { "br=|branch=", (string v) => branch = v},
                { "h|help", v => help = (v != null) }
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

        public bool ChangesetsFromBranchRequested()
        {
            return (branch != null);
        }

        public bool MergesRequested()
        {
            return merges;
        }

        public string GetRequestedBranch()
        {
            return branch;
        }

        public bool SourceBranchRequested()
        {
            return srcbranch != null;
        }

        public string GetRequestedSrcBranch()
        {
            return srcbranch;
        }

        public bool DestinationBranchRequested()
        {
            return dstbranch != null;
        }

        public string GetRequestedDstBranch()
        {
            return dstbranch;
        }

        public bool HelpRequested()
        {
            return help;
        }

        public static string GetHelp()
        {
            return helpStr;
        }
    }
}
