namespace Slack.API.Services
{
    public interface IPlasticCMDService
    {
        string GetLatestChangesets();

        string GetLatestBranches();

        string GetLicenseInfo();

        string SwitchToRepository(string requestedRepo);

        string FindMergeFromSrc(string requestedSrc);

        string FindMergeFromDst(string requestedDst);

        string ListRepositories();

        string ListLabels();
    }
}
