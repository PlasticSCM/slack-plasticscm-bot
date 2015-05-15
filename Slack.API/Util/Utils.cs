using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slack.API.Util
{
    public class Utils
    {   
        public static string FormatAsCode(string text)
        {
            StringBuilder sb = new StringBuilder(text.Length + 6);
            sb.Append(CodeMark);
            sb.Append(text);
            sb.Append(CodeMark);

            return sb.ToString();
        }

        public static string FormatBranchHistory(string text)
        {
            string[] rawLines = text.Split(new char[] { '\r', '\n' });
            List<string> nonEmptyLines = new List<string>();
            foreach (string line in rawLines)
            {
                if (line.Length > 0)
                    nonEmptyLines.Add(line);
            }
            StringBuilder sb = new StringBuilder();
            int i = (nonEmptyLines.Count > 25) ? nonEmptyLines.Count - 25 : 0;
            for (; i<nonEmptyLines.Count; i++)
            {
                sb.Append(nonEmptyLines.ElementAt(i));
                sb.Append("\n");
            }

            return FormatAsCode(sb.ToString());
        }

        const string CodeMark = "```";
    }
}
