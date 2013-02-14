using System;
using System.Collections.Generic;
using GitHubTfsSyncApp.Helpers;
using GitHubTfsSyncApp.Models.GitHub;

namespace GitHubTfsSyncApp.Controllers
{
	public class TfsSyncWorker
	{
		public void Process(IEnumerable<Commit> commits, Repository repository)
		{
			EnsureRepositoryIsQueryAble(repository);
		}

		private void EnsureRepositoryIsQueryAble(Repository repository)
		{
			if(repository == null)
				throw new ArgumentNullException("repository", "repository object cannot be null");

			var provider = new GitHubProvider(GitHubAccessConfigurationGenerator.CreateFromWebConfig());

			if(!provider.CanQueryRepo(repository.Url))
				throw new Exception("Repository is not reachable. Check your settings. For more info: http://developer.github.com/v3/oauth/#non-web-application-flow");
		}
	}
}