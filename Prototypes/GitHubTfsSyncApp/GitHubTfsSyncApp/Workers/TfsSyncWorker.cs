using System;
using System.Collections.Generic;
using System.Net;
using GitHubTfsSyncApp.Helpers;
using GitHubTfsSyncApp.Models;
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
		private readonly ICredentials _credentials;

		public TfsSyncWorker(string tfsUri, string teamProject, string localWorkspaceRootDir, ICredentials credentials)
		{
			_tfsUri = tfsUri;
			_teamProject = teamProject;
			_localWorkspaceRootDir = localWorkspaceRootDir;
			_credentials = credentials;
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

				ProcessTree(trees, provider, localDir, commitSpecs, commit);
			}

			var manager = new TfsManager(_tfsUri, _teamProject, new DirectoryInfo(_localWorkspaceRootDir), _credentials);

			manager.CreateCheckin(commitSpecs.ToArray());
		}

		private void ProcessTree(TreeCollection trees, GitHubProvider provider, DirectoryInfo localDir, List<CommitSpec> commitSpecs,
		                         Commit commit)
		{
			foreach (var node in trees.Tree)
			{
				if (node.Type == "blob")
				{
					var blob = provider.GetBlob(new Uri(node.Url));

					var filepath = Path.Combine(localDir.FullName, node.Path);

					SaveBlobToFileSystem(blob, filepath);

					commitSpecs.Add(new CommitSpec(commit) {LocalFilePath = filepath});
				}
				else if (node.Type == "tree")
				{
					var subDir = Directory.CreateDirectory(Path.Combine(localDir.FullName, node.Path));
					var subTrees = provider.GetTrees(new Uri(node.Url));

					ProcessTree(subTrees, provider, subDir, commitSpecs, commit);
				}
			}
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
	}
}