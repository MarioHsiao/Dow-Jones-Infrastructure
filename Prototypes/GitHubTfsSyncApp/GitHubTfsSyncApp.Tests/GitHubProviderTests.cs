using System;
using GitHubTfsSyncApp.Controllers;
using GitHubTfsSyncApp.Models.GitHub;
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

		[TestMethod]
		public void CanGetCommitDetails()
		{
			// Licensed under Demo Exception: Bad test - relies on magic data.

			var provider = new GitHubProvider(new GitHubAccessConfiguration
			{
				Credentials = Settings1.Default.GitHubCredentials,
				ClientId = Settings1.Default.GitHubClientId,
				ClientSecret = Settings1.Default.GitHubClientSecret,
				EndPoint = Settings1.Default.GitHubApiEndPoint
			});

			var response = provider.GetCommitDetails(new Commit { Id = "56ba6889a8a90f2980de4c7a531f544d0050ac8c" }, "TfsSyncDemo", "pandah");

			Assert.IsNotNull(response, "Couldn't fetch Commit Details");
			Assert.AreEqual("4e6aec65517ddf1ea29d3ef335ac8c6775775174", response.Tree.Sha);
			
		}

		[TestMethod]
		public void CanGetTrees()
		{
			// Licensed under Demo Exception: Bad test - relies on magic data.

			var provider = new GitHubProvider(new GitHubAccessConfiguration
			{
				Credentials = Settings1.Default.GitHubCredentials,
				ClientId = Settings1.Default.GitHubClientId,
				ClientSecret = Settings1.Default.GitHubClientSecret,
				EndPoint = Settings1.Default.GitHubApiEndPoint
			});

			var response = provider.GetCommitDetails(new Commit { Id = "56ba6889a8a90f2980de4c7a531f544d0050ac8c" }, "TfsSyncDemo", "pandah");
			Assert.IsNotNull(response, "Couldn't fetch Commit Details");
			Assert.IsNotNull(response.Tree, "Couldn't fetch Commit Detail Tree");

			var trees = provider.GetTrees(new Uri(response.Tree.Url));

			Assert.IsNotNull(trees, "Couldn't fetch Trees");
			Assert.AreEqual("4e6aec65517ddf1ea29d3ef335ac8c6775775174", trees.Sha);
			Assert.AreEqual(4, trees.Tree.Count);

		}
	}
}
