namespace Slack.API.Services
{
    public interface IPlasticCMDService
    {
        string GetLatestChangesets();

        string GetLatestBranches();

        string GetLicenseInfo();
    }
}
