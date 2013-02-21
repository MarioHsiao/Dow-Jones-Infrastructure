using System;
using System.IO;
using System.Net;
using AttributeRouting.Helpers;
using GitHubTfsSyncApp.Extensions;
using GitHubTfsSyncApp.Models;
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
			_versionControl.NonFatalError += OnVersionControlOnNonFatalError;
		}

		private void OnVersionControlOnNonFatalError(object o, ExceptionEventArgs args)
		{
			_logger.Error(args.Failure.Message, args.Exception);
		}

		public void CreateCheckin(ChangedItems changedItems, string workspaceName, string checkinMessage)
		{
			var workspaceMappedPath = Path.Combine(_workingDir.FullName, workspaceName);
			var workspace = InitializeWorkspace(workspaceName, workspaceMappedPath);

			if (workspace == null) return;

			CommitInWorkspace(changedItems, workspace, workspaceMappedPath);
			ResolveConflicts(workspace);

			var pendingChanges = workspace.GetPendingChanges();

			if (pendingChanges.Length == 0)
				_logger.Info("No changes to checkin");
			else
			{
				var changesetNumber = workspace.CheckIn(pendingChanges, checkinMessage);
				
				_logger.Info(changesetNumber == 0
					             ? "No new changes."
					             : "Checked in changeset {0}.\nCheckin Summary: {1}".FormatWith(changesetNumber, checkinMessage));
			}

			workspace.Delete();
			new DirectoryInfo(workspaceMappedPath).ForceDelete();
		}

		private void CommitInWorkspace(ChangedItems changedItems, Workspace workspace, string workspaceMappedPath)
		{
			foreach (var item in changedItems.Added)
			{
				File.Copy(item.IncomingFilePath, Path.Combine(workspaceMappedPath, item.GitPath), true);
				workspace.PendAdd(Path.Combine(workspaceMappedPath, item.GitPath));
			}

			foreach (var item in changedItems.Modified)
			{
				// checkout file first
				workspace.PendEdit(Path.Combine(workspaceMappedPath, item.GitPath));

				// apply external changes
				File.Copy(item.IncomingFilePath, Path.Combine(workspaceMappedPath, item.GitPath), true);
			}

			foreach (var item in changedItems.Deleted)
				workspace.PendDelete(Path.Combine(workspaceMappedPath, item.GitPath));
			
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

		public Workspace InitializeWorkspace(string name, string mappedFolderPath)
		{
			var versionControl = _versionControl;

			EnsureWorkspaceIsNotStale(versionControl, name);

			var workspace = versionControl.CreateWorkspace(name, versionControl.AuthorizedUser);

			workspace.Map(_teamProjectPath, mappedFolderPath);
			workspace.Get();

			return workspace;

		}

		private void EnsureWorkspaceIsNotStale(VersionControlServer versionControl, string name)
		{
			try
			{
				var existingWorkspace = versionControl.GetWorkspace(name, versionControl.AuthorizedUser);

				// if it got here, it means the workspace exists
				existingWorkspace.Delete();
			}
			catch (WorkspaceNotFoundException)
			{
				// do nothing
				// could have been much better if the API allowed some query to check for existing workspace
			}
		}
	}
}