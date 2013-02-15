using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		private readonly Lazy<TfsTeamProjectCollection> _teamProjectCollection;
		private readonly Lazy<VersionControlServer> _versionControl;


		/// <summary>
		/// Initializes a new instance of the <see cref="TfsManager"/> class.
		/// </summary>
		/// <param name="tfsUrl">The TFS URL. E.g. http://sbknwstfs1:8080/tfs </param>
		/// <param name="teamProjectPath">The team project path. E.g. $/Dow Jones Infrastructure </param>
		/// <param name="workingDir">Local working directory. E.g. C:\Workspaces </param>
		public TfsManager(string tfsUrl, string teamProjectPath, DirectoryInfo workingDir)
		{
			_tfsUrl = tfsUrl;
			_teamProjectPath = teamProjectPath;
			_workingDir = workingDir;

			_teamProjectCollection = new Lazy<TfsTeamProjectCollection>(() => new TfsTeamProjectCollection(new Uri(_tfsUrl)));
			_versionControl = new Lazy<VersionControlServer>(() => _teamProjectCollection.Value.GetService<VersionControlServer>());
		}

		public void CreateCheckin(Commit[] commits)
		{
			// do not parallelize this.
			// keeping it sequential is essentially same as git rebasing. 
			foreach (var commit in commits)
				ProcessCommit(commit);
		}

		private void ProcessCommit(Commit commit)
		{
			var workspace = InitializeWorkspace(commit.Id);
			var incomingDir = Path.Combine(_workingDir.FullName, "incoming", commit.Id);

			foreach (var fileName in commit.Added)
				workspace.PendAdd(Path.Combine(incomingDir, fileName));

			foreach (var fileName in commit.Modified)
				workspace.PendEdit(Path.Combine(incomingDir, fileName));

			foreach (var fileName in commit.Removed)
				workspace.PendDelete(Path.Combine(incomingDir, fileName));

			ResolveConflicts(workspace);

			var pendingChanges = workspace.GetPendingChanges();

			var checkinMessage = CreateCheckinMessage(commit);
			var changesetNumber = workspace.CheckIn(pendingChanges, checkinMessage);

			_logger.Info("Checked in changeset# {0}.\nCheckin Summary: {1}".FormatWith(changesetNumber, checkinMessage));
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

		private static string CreateCheckinMessage(Commit commit)
		{
			const string template = "Automated checkin created by TfsSyncApp.\n\n"
									+ "Message: {0}\n\n"
									+ "{1} additions, {2} modifications, {3} deletions\n"
									+ "Timestamp: {4}\n"
									+ "Author: {5}\n"
									+ "Committer: {6}\n"
									+ "Url: {7}";

			return template.FormatWith(template,
								commit.Message,
								commit.Added.Count(),
								commit.Modified.Count(),
								commit.Removed.Count(),
								commit.Timestamp.ToString(),
								commit.Author.Email,
								commit.Committer.Email,
								commit.Url);
		}

		private Workspace InitializeWorkspace(string name)
		{
			var versionControl = _versionControl.Value;
			var workspace = versionControl.CreateWorkspace(name, versionControl.AuthorizedUser);

			workspace.Map(_teamProjectPath, _workingDir.FullName);
			workspace.Get();

			return workspace;
		}
	}
}