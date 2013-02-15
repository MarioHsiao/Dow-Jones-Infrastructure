using System;
using System.Collections.Generic;
using AttributeRouting.Helpers;
using GitHubTfsSyncApp.Helpers;
using GitHubTfsSyncApp.Models.GitHub;
using GitHubTfsSyncApp.Providers;
using System.IO;
using System.Linq;

namespace GitHubTfsSyncApp.Workers
{
	public class TfsSyncWorker
	{
		private readonly string _tfsUri;
		private readonly string _teamProject;
		private readonly string _localWorkspaceRootDir;

		public TfsSyncWorker(string tfsUri, string teamProject, string localWorkspaceRootDir)
		{
			_tfsUri = tfsUri;
			_teamProject = teamProject;
			_localWorkspaceRootDir = localWorkspaceRootDir;
		}

		public void Process(IEnumerable<Commit> commits, Repository repository)
		{
			var commitSpecs = new List<CommitSpec>();
			var provider = new GitHubProvider(GitHubAccessConfigurationGenerator.CreateFromWebConfig());

			foreach (var commit in commits)
			{
				var localDir = Directory.CreateDirectory(Path.Combine(_localWorkspaceRootDir, "incoming", commit.Id));

				var details = provider.GetCommitDetails(commit, repository.Name, repository.Owner.Name);
				var trees = provider.GetTrees(new Uri(details.Tree.Url));

				foreach (var node in trees.Tree.Where(x => x.Type == "blob"))
				{
					var blob = provider.GetBlob(new Uri(node.Url));
					var filepath = Path.Combine(localDir.FullName, node.Path);

					SaveBlobToFileSystem(blob, filepath);

					commitSpecs.Add(new CommitSpec(commit) { LocalFilePath = filepath });
				}
			}

			var manager = new TfsManager(_tfsUri, _teamProject, new DirectoryInfo(_localWorkspaceRootDir));

			manager.CreateCheckin(commitSpecs.ToArray());
		}

		private void SaveBlobToFileSystem(Blob blob, string fileName)
		{
			var bytes = Convert.FromBase64String(blob.Content);
			var fi = new FileInfo(fileName);
			using (var stream = fi.OpenWrite())
			{
				stream.Write(bytes, 0, bytes.Length);
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
		public CommitSpec(Commit commit)
		{
			Added = commit.Added;
			Author = commit.Author;
			Committer = commit.Committer;
			Distinct = commit.Distinct;
			Id = commit.Id;
			Message = commit.Message;
			Modified = commit.Modified;
			Removed = commit.Removed;
			Timestamp = commit.Timestamp;
			Url = commit.Url;
		}

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