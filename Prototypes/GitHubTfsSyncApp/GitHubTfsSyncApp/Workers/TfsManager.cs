using System;
using System.IO;
using System.Net;
using System.Security.AccessControl;
using System.Web.Providers.Entities;
using AttributeRouting.Helpers;
using GitHubTfsSyncApp.Extensions;
using GitHubTfsSyncApp.Helpers;
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
        private readonly NetworkCredential _credential;


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
            _credential = (NetworkCredential)credentials;
            _teamProjectCollection = new TfsTeamProjectCollection(new Uri(_tfsUrl), _credential);
            _versionControl = _teamProjectCollection.GetService<VersionControlServer>();
            _versionControl.NonFatalError += OnVersionControlOnNonFatalError;
            _logger.Info("Intialize Tfs Manager");
        }

		private void OnVersionControlOnNonFatalError(object o, ExceptionEventArgs args)
		{
            if (args != null && args.Failure != null)
			    _logger.Error(args.Failure.Message, args.Exception);
		}

		public void CreateCheckin(ChangedItems changedItems, string workspaceName, string checkinMessage)
		{
		    try
		    {
		        var workspaceMappedPath = Path.Combine(_workingDir.FullName, workspaceName);
		        var workspace = InitializeWorkspace(workspaceName, workspaceMappedPath);
                
		        if (workspace == null) return;

                _logger.Info("Commiting workspace");
		        CommitInWorkspace(changedItems, workspace, workspaceMappedPath);
                _logger.Info("Resolving conflict");
		        ResolveConflicts(workspace);
                _logger.Info("Getting pending changes");
		        var pendingChanges = workspace.GetPendingChanges();

		        if (pendingChanges.Length == 0)
		            _logger.Info("No changes to checkin");
		        else
		        {
                    _logger.Info("checkin workspaces");
		            var changesetNumber = workspace.CheckIn(pendingChanges, checkinMessage);

		            _logger.Info(changesetNumber == 0
		                             ? "No new changes."
		                             : "Checked in changeset {0}.\nCheckin Summary: {1}".FormatWith(changesetNumber,
		                                                                                            checkinMessage));
                    _logger.Info(changesetNumber);
		        }
                _logger.Info("delting workspace");
		        workspace.Delete();
                _logger.Info("workspace deleted");
                _logger.Info(workspaceMappedPath);
		        new DirectoryInfo(workspaceMappedPath).ForceDelete();
		    }
		    catch (Exception exception)
		    {
		        _logger.Info("Error while checkin");
                 _logger.Error(exception);
		        throw;
		    }
		}

	    public static void AddFileSecurity(string fileName, string account,
	                                       FileSystemRights rights, AccessControlType controlType)
	    {
	        // Get a FileSecurity object that represents the 
	        // current security settings.
	        FileSecurity fSecurity = File.GetAccessControl(fileName);

	        // Add the FileSystemAccessRule to the security settings.
	        fSecurity.AddAccessRule(new FileSystemAccessRule(account,
	                                                         rights, controlType));

	        // Set the new access settings.
	        File.SetAccessControl(fileName, fSecurity);
            //removing readonly attribute of file
            File.SetAttributes(fileName,FileAttributes.Normal);

	    }

	    private void CommitInWorkspace(ChangedItems changedItems, Workspace workspace, string workspaceMappedPath)
		{
	        try
	        {
			    foreach (var item in changedItems.Added)
			    {
                    _logger.Info("adding file security");
                    //Giving full permissions to the user for copying the file
                    AddFileSecurity(Path.Combine(workspaceMappedPath, item.GitPath),_credential.Domain+"\\"+_credential.UserName, FileSystemRights.Read | FileSystemRights.ReadAndExecute | FileSystemRights.Write | FileSystemRights.FullControl, AccessControlType.Allow);
                    _logger.Info("copying files");
				    File.Copy(item.IncomingFilePath, Path.Combine(workspaceMappedPath, item.GitPath), true);
                    _logger.Info("workspace add");
				    workspace.PendAdd(Path.Combine(workspaceMappedPath, item.GitPath));
			    }

			    foreach (var item in changedItems.Modified)
			    {
                    _logger.Info("adding file security");
                    AddFileSecurity(Path.Combine(workspaceMappedPath, item.GitPath), _credential.Domain + "\\" + _credential.UserName, FileSystemRights.Read | FileSystemRights.ReadAndExecute | FileSystemRights.Write | FileSystemRights.FullControl, AccessControlType.Allow);
                    _logger.Info("editing files");
                    // checkout file first
				    workspace.PendEdit(Path.Combine(workspaceMappedPath, item.GitPath));
                    _logger.Info("copying files");
				    // apply external changes
				    File.Copy(item.IncomingFilePath, Path.Combine(workspaceMappedPath, item.GitPath), true);
			    }

			    foreach (var item in changedItems.Deleted)
				    workspace.PendDelete(Path.Combine(workspaceMappedPath, item.GitPath));
            }
            catch (Exception ex)
            {
                _logger.Info("Error while comminting");
                _logger.Error(ex);
                throw;
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