using System.Configuration;
using GitHubTfsSyncApp.Controllers;

namespace GitHubTfsSyncApp.Helpers
{
	public class GitHubAccessConfigurationGenerator
	{
		public static GitHubAccessConfiguration CreateFromWebConfig()
		{
			return new GitHubAccessConfiguration
				{
					Credentials = ConfigurationManager.AppSettings.Get("GitHub:Credentials"),
					ClientId = ConfigurationManager.AppSettings.Get("GitHub:ClientId"),
					ClientSecret = ConfigurationManager.AppSettings.Get("GitHub:ClientSecret"),
					EndPoint = ConfigurationManager.AppSettings.Get("GitHub:ApiEndPoint"),
				};
		}
	}
}