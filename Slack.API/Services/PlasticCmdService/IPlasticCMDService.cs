namespace Slack.API.Services
{
    public interface IPlasticCMDService
    {
        string GetLatestChangesets();

        string GetLatestChangesetsFromBranch(string requestedBranch);

        string GetLatestBranches();

        string GetLicenseInfo();

        string SwitchToRepository(string requestedRepo);

        string FindMergeFromSrc(string requestedSrc);

        string FindMergeFromDst(string requestedDst);

        string ListRepositories();

        string ListLabels();

        string ListLabelsInBranch(string requestedBranch);

        string BranchHistory(string requestedBranch);

        string BranchHistory(string requestedBranch, string requestedRep);

        string HalStatus();
    }
}
