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

		public TreeCollection GetTrees(string sha, string repo, string owner)
		{
			// GET /repos/:owner/:repo/git/trees/:sha

			var resource = string.Format("/repos/{0}/{1}/git/trees/{2}", owner, repo, sha);

			var restApiProvider = new RestApiProvider(_gitHubAccesConfiguration.EndPoint);

			var response = restApiProvider.Get<TreeCollection>(resource, new Dictionary<string, string> { { "Authorization", _gitHubAccesConfiguration.Credentials } });

			return response;
		}

		public bool CanQueryRepo(string repoUrl)
		{
			throw new NotImplementedException();
		}
	}

	public class CommitDetails
	{
		public Author Committer { get; set; }
		public List<GitPointer> Parents { get; set; }
		public string Message { get; set; }
		public string Sha { get; set; }
		public GitPointer Tree { get; set; }
		public string Url { get; set; }
		public Author Author { get; set; }
	}

	public class GitPointer
	{
		public string Sha { get; set; }
		public string Url { get; set; }

	}

	public class Tree
	{
		public string Type { get; set; }
		public string Sha { get; set; }
		public string Url { get; set; }
		public int Size { get; set; }
		public string Path { get; set; }
		public string Mode { get; set; }
	}

	public class TreeCollection
	{
		public List<Tree> Tree { get; set; }
		public string Sha { get; set; }
		public string Url { get; set; }
	}
}