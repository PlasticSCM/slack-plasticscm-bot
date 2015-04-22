using Mono.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Example of use
//
// plastic find merge dstbranch=/main
// plastic find merge srcbranch=/main/task001
namespace Slack.API.Model.Commands
{
    public class PlasticFindMerge : BaseCommand
    {
        private string dstbranch;
        private string srcbranch;
        private bool help;
        private static string helpStr =
            @"`plastic find merge -dstbranch=/main`
`plastic find merge -srcbranch=/main/task001`";

        public PlasticFindMerge(string requestedCommand)
        {
            string[] args = ExtractArgs(requestedCommand);
            new OptionSet()
            {
                { "dstbranch=", (string v) => dstbranch = v },
                { "srcbranch=", (string v) => srcbranch = v },
                { "h|help", v => help = (v != null) }
            }.Parse(args);
        }

        public bool IsDstBranch()
        {
            return dstbranch != null;
        }

        public bool IsSrcBranch()
        {
            return srcbranch != null;
        }

        public string GetDstBranch()
        {
            return dstbranch;
        }

        public string GetSrcBranch()
        {
            return srcbranch;
        }

        public bool IsHelp()
        {
            return help;
        }

        public static string getHelp()
        {
            return helpStr;
        }
    }
}
