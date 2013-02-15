using System;
using System.Collections.Generic;
using GitHubTfsSyncApp.Controllers;
using GitHubTfsSyncApp.Models.GitHub;

namespace GitHubTfsSyncApp.Providers
{
	public class GitHubProvider
	{
		private readonly GitHubAccessConfiguration _gitHubAccesConfiguration;

		public GitHubProvider(GitHubAccessConfiguration gitHubAccesConfiguration)
		{
			_gitHubAccesConfiguration = gitHubAccesConfiguration;
		}

		public string GetOAuthToken()
		{
			var restApiProvider = new RestApiProvider(_gitHubAccesConfiguration.EndPoint);

			var response = restApiProvider.Post<AuthorizationResponse>("/authorizations", new
			{
				client_id = _gitHubAccesConfiguration.ClientId,
				client_secret = _gitHubAccesConfiguration.ClientSecret
			}, new Dictionary<string, string> { { "Authorization", _gitHubAccesConfiguration.Credentials } });

			return response.Token;
		}

		public CommitDetails GetCommitDetails(Commit commit, string repo, string owner)
		{
			// GET /repos/:owner/:repo/git/commits/:sha

			var resource = string.Format("/repos/{0}/{1}/git/commits/{2}", owner, repo, commit.Id);

			var restApiProvider = new RestApiProvider(_gitHubAccesConfiguration.EndPoint);

			var response = restApiProvider.Get<CommitDetails>(resource, new Dictionary<string, string> { { "Authorization", _gitHubAccesConfiguration.Credentials } });

			return response;
		}

		public TreeCollection GetTrees(Uri uri)
		{
			// GET /repos/:owner/:repo/git/trees/:sha
			var response = RestApiProvider.Get<TreeCollection>(uri, new Dictionary<string, string> { { "Authorization", _gitHubAccesConfiguration.Credentials } });

			return response;
		}

		public Blob GetBlob(Uri uri)
		{
			var response = RestApiProvider.Get<Blob>(uri, new Dictionary<string, string> { { "Authorization", _gitHubAccesConfiguration.Credentials } });
			return response;
		}

		public bool CanQueryRepo(string repoUrl)
		{
			throw new NotImplementedException();
		}
	}
}