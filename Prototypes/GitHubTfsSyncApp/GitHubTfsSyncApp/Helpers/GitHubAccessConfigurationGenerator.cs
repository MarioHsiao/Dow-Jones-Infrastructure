using GitHubTfsSyncApp.Controllers;

namespace GitHubTfsSyncApp.Helpers
{
	public class GitHubAccessConfigurationGenerator
	{
        public static GitHubAccessConfiguration CreateFromWebConfig(string credentials, string clientId, string clientSecret, string apiEndPoint)
        {
            return new GitHubAccessConfiguration
            {
                Credentials = credentials,
                ClientId = clientId,
                ClientSecret = clientSecret,
                EndPoint = apiEndPoint
            };
        }
	}
}