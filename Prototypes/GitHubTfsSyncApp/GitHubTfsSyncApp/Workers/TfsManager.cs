using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Web;
using AttributeRouting.Helpers;
using GitHubTfsSyncApp.Models.GitHub;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using log4net;

namespace GitHubTfsSyncApp.Workers
{
	public class TfsManager
	{
		private readonly ILog _logger = LogManager.GetLogger(typeof(TfsManager));

		private readonly string _tfsUrl;
		private readonly string _teamProjectPath;
		private readonly DirectoryInfo _workingDir;
		//private readonly Lazy<TfsTeamProjectCollection> _teamProjectCollection;
		//private readonly Lazy<VersionControlServer> _versionControl;
		private readonly TfsTeamProjectCollection _teamProjectCollection;
		private readonly VersionControlServer _versionControl;


		/// <summary>
		/// Initializes a new instance of the <see cref="TfsManager"/> class.
		/// </summary>
		/// <param name="tfsUrl">The TFS URL. E.g. http://sbknwstfs1:8080/tfs </param>
		/// <param name="teamProjectPath">The team project path. E.g. $/Dow Jones Infrastructure </param>
		/// <param name="workingDir">Local working directory. E.g. C:\Workspaces </param>
		/// <param name="credentials">Credentials to access TFS</param>
		public TfsManager(string tfsUrl, string teamProjectPath, DirectoryInfo workingDir, ICredentials credentials)
		{
			_tfsUrl = tfsUrl;
			_teamProjectPath = teamProjectPath;
			_workingDir = workingDir;

			_teamProjectCollection = new TfsTeamProjectCollection(new Uri(_tfsUrl), credentials);
			_versionControl = _teamProjectCollection.GetService<VersionControlServer>();
		}

		public void CreateCheckin(CommitSpec[] commits)
		{
			// do not parallelize this.
			// keeping it sequential is essentially same as git rebasing. 
			foreach (var commit in commits)
				ProcessCommit(commit);
		}

		private void ProcessCommit(CommitSpec commit)
		{
			var incomingDir = Path.Combine(_workingDir.FullName, "incoming", commit.Id);
			var workspaceName = "Workspace_{0}".FormatWith(commit.Id);
			var workspaceMappedFolderPath = Path.Combine(_workingDir.FullName, workspaceName);


			var workspace = InitializeWorkspace(workspaceName, workspaceMappedFolderPath);

			try
			{
				foreach (var fileName in commit.Added)
				{
					File.Copy(Path.Combine(incomingDir, fileName), Path.Combine(workspaceMappedFolderPath, fileName));
					workspace.PendAdd(Path.Combine(workspaceMappedFolderPath, fileName));
				}

				foreach (var fileName in commit.Modified)
				{
					// checkout file first
					workspace.PendEdit(Path.Combine(workspaceMappedFolderPath, fileName));

					// apply external changes
					File.Copy(Path.Combine(incomingDir, fileName), Path.Combine(workspaceMappedFolderPath, fileName), true);
				}

				foreach (var fileName in commit.Removed)
				{
					workspace.PendDelete(Path.Combine(workspaceMappedFolderPath, fileName));
				}

				ResolveConflicts(workspace);

				var pendingChanges = workspace.GetPendingChanges();

				var checkinMessage = commit.ToSummary();
				var changesetNumber = workspace.CheckIn(pendingChanges, checkinMessage);

				_logger.Info("Checked in changeset# {0}.\nCheckin Summary: {1}".FormatWith(changesetNumber, checkinMessage));

			}
			catch (Exception ex)
			{
				_logger.Error("Error processing commit #{0}".FormatWith(commit.Id), ex);
			}
			finally
			{
				// cleanup
				workspace.Delete();
			}
		}

		private void ResolveConflicts(Workspace workspace)
		{
			var conflicts = workspace.QueryConflicts(new[] { _workingDir.FullName }, true);

			foreach (var conflict in conflicts)
			{
				conflict.Resolution = Resolution.AcceptTheirs;
				workspace.ResolveConflict(conflict);
			}
		}

		private Workspace InitializeWorkspace(string name, string mappedFolderPath)
		{
			var versionControl = _versionControl;
			var workspace = versionControl.CreateWorkspace(name, versionControl.AuthorizedUser);

			workspace.Map(_teamProjectPath, mappedFolderPath);
			workspace.Get();

			return workspace;
		}
	}
}