using System;
using GitHubTfsSyncApp.Controllers;
using GitHubTfsSyncApp.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitHubTfsSyncApp.Tests
{
	[TestClass]
	public class GitHubProviderTests
	{
		[TestMethod]
		public void CanGetOAuthToken()
		{
			var provider = new GitHubProvider(new GitHubAccessConfiguration
				{
					Credentials = Settings1.Default.GitHubCredentials,
					ClientId = Settings1.Default.GitHubClientId,
					ClientSecret = Settings1.Default.GitHubClientSecret,
					EndPoint = Settings1.Default.GitHubApiEndPoint
				});

			var response = provider.GetOAuthToken();

			Assert.IsTrue(!string.IsNullOrEmpty(response), "Null or Empty token received");
		}
	}
}
