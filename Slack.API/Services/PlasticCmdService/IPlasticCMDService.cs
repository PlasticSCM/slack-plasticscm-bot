namespace Slack.API.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPlasticCMDService
    {
        string GetLatestChangesets();

        string GetLatestBranches();
    }
}
