using System;
using System.Collections.Generic;
using AttributeRouting.Helpers;
using GitHubTfsSyncApp.Controllers;
using GitHubTfsSyncApp.Helpers;
using GitHubTfsSyncApp.Models.GitHub;
using GitHubTfsSyncApp.Providers;
using System.Diagnostics;
using System.IO;
using System.Linq;
//using Microsoft.TeamFoundation.Client;
//using Microsoft.TeamFoundation.VersionControl.Client;

namespace GitHubTfsSyncApp.Workers
{
	public class TfsSyncWorker
	{
		private readonly string _tfsUri;

		public TfsSyncWorker(string tfsUri)
		{
			_tfsUri = tfsUri;

		}

		public void Process(IEnumerable<Commit> commits, Repository repository)
		{
			var commitSpecs = new List<CommitSpec>();
			var provider = new GitHubProvider(GitHubAccessConfigurationGenerator.CreateFromWebConfig());

			foreach (var commit in commits)
			{
				var details = provider.GetCommitDetails(commit, repository.Name, repository.Owner.Name);
			}
		}

		private void EnsureRepositoryIsQueryAble(Repository repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository", "repository object cannot be null");

			var provider = new GitHubProvider(GitHubAccessConfigurationGenerator.CreateFromWebConfig());

			if (!provider.CanQueryRepo(repository.Url))
				throw new Exception("Repository is not reachable. Check your settings. For more info: http://developer.github.com/v3/oauth/#non-web-application-flow");
		}
	}

	public class CommitSpec : Commit
	{
		public string LocalFilePath { get; set; }

		public string ToSummary()
		{
			const string template = "Message: {0}\n\n"
									+ "{1} additions, {2} modifications, {3} deletions\n"
									+ "Timestamp: {4}\n"
									+ "Author: {5}\n"
									+ "Committer: {6}\n"
									+ "Url: {7}";

			return template.FormatWith(template,
									   Message,
									   Added.Count(),
									   Modified.Count(),
									   Removed.Count(),
									   Timestamp.ToString(),
									   Author.Email,
									   Committer.Email,
									   Url);
		}
	}
}