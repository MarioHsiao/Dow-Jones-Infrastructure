using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using AttributeRouting.Helpers;
using GitHubTfsSyncApp.Helpers;
using GitHubTfsSyncApp.Models;
using GitHubTfsSyncApp.Models.GitHub;
using GitHubTfsSyncApp.Providers;
using System.IO;
using System.Linq;
using log4net;
using GitHubTfsSyncApp.Extensions;

namespace GitHubTfsSyncApp.Workers
{
	public class TfsSyncWorker
	{
		private readonly string _tfsUri;
		private readonly string _teamProject;
		private readonly string _localWorkspaceRootDir;
		private readonly ICredentials _credentials;
		private readonly ILog _logger = LogManager.GetLogger(typeof(TfsManager));
		private readonly GitHubProvider _gitHubProvider;
		private readonly TfsManager _tfsManager;


		public TfsSyncWorker(string tfsUri, string teamProject, string localWorkspaceRootDir, ICredentials credentials)
		{
			_tfsUri = tfsUri;
			_teamProject = teamProject;
			_localWorkspaceRootDir = localWorkspaceRootDir;
			_credentials = credentials;

			_gitHubProvider = new GitHubProvider(GitHubAccessConfigurationGenerator.CreateFromWebConfig());
			_tfsManager = new TfsManager(_tfsUri, _teamProject, new DirectoryInfo(_localWorkspaceRootDir), _credentials);
		}

		public void Process(IEnumerable<Commit> commits, Repository repository)
		{
			try
			{
				foreach (var commit in commits)
				{
					//TODO: eliminate magic strings
					var localDir = Directory.CreateDirectory(Path.Combine(_localWorkspaceRootDir, "Incoming", commit.Id));

					var details = _gitHubProvider.GetCommitDetails(commit, repository.Name, repository.Owner.Name);

					// get tree/subtrees in one shot
					var trees = _gitHubProvider.GetTrees(new Uri(details.Tree.Url + "?recursive=1"));

					// save blobs, figure out changes
					var changes = ProcessTree(trees.Tree, localDir.FullName, commit);

					// checkin those changes
					_tfsManager.CreateCheckin(changes, "Workspace_{0}".FormatWith(commit.Id), commit.Summary);

					// cleanup
					localDir.ForceDelete();
				}

			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message, ex);
			}

		}

		private ChangedItems ProcessTree(IEnumerable<TreeNode> treeNodes, string localDirPath, Commit commit)
		{
			var realizedNodes = treeNodes.Where(x => x.Type == "blob").ToArray();

			var changedItems = new ChangedItems
				{
					Added = ProcessNodes(localDirPath, realizedNodes, commit.Added),
					Modified = ProcessNodes(localDirPath, realizedNodes, commit.Modified),
					Deleted = commit.Removed.Select(x => new WorkspaceItem { GitPath = x }),
				};

			return changedItems;
		}

		/// <summary>
		/// Saves the blobs in given nodes.
		/// </summary>
		/// <param name="localDirPath">The local dir.</param>
		/// <param name="nodes">The nodes.</param>
		/// <param name="paths">The paths.</param>
		/// <returns>Local file path of saved blobs.</returns>
		private IEnumerable<WorkspaceItem> ProcessNodes(string localDirPath, IEnumerable<TreeNode> nodes, IEnumerable<string> paths)
		{
			var realizedPaths = paths as string[] ?? paths.ToArray();

			if (!realizedPaths.Any())
				return Enumerable.Empty<WorkspaceItem>();

			var workspaceItems = new List<WorkspaceItem>();

			foreach (var node in nodes.Where(x => realizedPaths.Contains(x.Path)))
			{
				var blob = _gitHubProvider.GetBlob(new Uri(node.Url));

				var filePath = Path.Combine(localDirPath, node.Path.Replace("/", "\\"));

				SaveBlobToFileSystem(blob, filePath);

				workspaceItems.Add(new WorkspaceItem
					{
						IncomingFilePath = filePath,
						GitPath = node.Path.Replace("/", "\\"),
					});
			}

			return workspaceItems.ToArray();
		}

		private void SaveBlobToFileSystem(Blob blob, string filePath)
		{
			var bytes = Convert.FromBase64String(blob.Content);
			
			var fi = new FileInfo(filePath);

			// create folders if not there
			if(fi.Directory != null && !fi.Directory.Exists)
				fi.Directory.Create();
			
			using (var stream = fi.Open(FileMode.Create, FileAccess.ReadWrite))
			{
				stream.Write(bytes, 0, bytes.Length);
			}
		}
	}
}