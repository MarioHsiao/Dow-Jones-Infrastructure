using System;
using System.Collections.Generic;
using System.Configuration;
using GitHubTfsSyncApp.Models.GitHub;

namespace GitHubTfsSyncApp.Controllers
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

			var response = restApiProvider.Post<AuthorizationResponse>("/authorizations", new {
				client_id = _gitHubAccesConfiguration.ClientId,
				client_secret = _gitHubAccesConfiguration.ClientSecret
			}, new Dictionary<string, string> { { "Authorization", _gitHubAccesConfiguration.Credentials } });

			return response.Token;
		}

		public bool CanQueryRepo(string repoUrl)
		{
			throw new NotImplementedException();
		}
	}
}