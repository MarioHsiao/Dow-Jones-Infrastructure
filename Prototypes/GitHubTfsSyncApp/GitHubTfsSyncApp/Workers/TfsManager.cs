using System;
using System.IO;
using System.Net;
using System.Security.AccessControl;
using AttributeRouting.Helpers;
using GitHubTfsSyncApp.Configuration;
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
        private readonly NetworkCredential _credential;
        private readonly FiltersCollection _filter;


	    /// <summary>
	    /// Initializes a new instance of the <see cref="TfsManager"/> class.
	    /// </summary>
	    /// <param name="tfsUrl">The TFS URL. E.g. http://sbknwstfs1:8080/tfs </param>
	    /// <param name="teamProjectPath">The team project path. E.g. $/Dow Jones Infrastructure </param>
	    /// <param name="workingDir">Local working directory. E.g. C:\Workspaces </param>
	    /// <param name="filter"></param>
	    /// <param name="credentials">Credentials to access TFS</param>
	    public TfsManager(string tfsUrl, string teamProjectPath, DirectoryInfo workingDir,FiltersCollection filter, ICredentials credentials)
        {
            _tfsUrl = tfsUrl;
            _teamProjectPath = teamProjectPath;
            _workingDir = workingDir;
            _credential = (NetworkCredential)credentials;
		    _filter = filter;
            _teamProjectCollection = new TfsTeamProjectCollection(new Uri(_tfsUrl), _credential);
            _versionControl = _teamProjectCollection.GetService<VersionControlServer>();
            _versionControl.NonFatalError += OnVersionControlOnNonFatalError;
            _logger.Info("Intialize Tfs Manager");
        }

		private void OnVersionControlOnNonFatalError(object o, ExceptionEventArgs args)
		{
		    try
		    {
		        _logger.Error(args.Exception);
                 if (args.Failure != null)
                    _logger.Error(args.Failure.Message, args.Exception);
		    }
		    catch (Exception ex)
		    {
                _logger.Info("error in OnVersionControlOnNonFatalError"+ex);
		    }
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
	        if (File.Exists(fileName))
	        {
	            FileSecurity fSecurity = File.GetAccessControl(fileName);

	            // Add the FileSystemAccessRule to the security settings.
	            fSecurity.AddAccessRule(new FileSystemAccessRule(account,
	                                                             rights, controlType));

	            // Set the new access settings.
	            File.SetAccessControl(fileName, fSecurity);
	            //removing readonly attribute of file
	            File.SetAttributes(fileName, FileAttributes.Normal);
	        }

	    }

	    private void CommitInWorkspace(ChangedItems changedItems, Workspace workspace, string workspaceMappedPath)
		{
	        try
	        {
                _logger.Info("workspaceMappedPath" + workspaceMappedPath);
			    foreach (var item in changedItems.Added)
			    {
                    if (_filter != null && _filter.Count > 0 && !IsFilterItem(item))
                        continue;
			        string filePath = Path.Combine(workspaceMappedPath, item.GitPath);
			        string directory = Path.GetDirectoryName(filePath);
                    if (directory != null && !Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    AddFileSecurity(Path.Combine(workspaceMappedPath, item.GitPath), _credential.Domain + "\\" + _credential.UserName, FileSystemRights.Read | FileSystemRights.ReadAndExecute | FileSystemRights.Write | FileSystemRights.FullControl, AccessControlType.Allow);
				    File.Copy(item.IncomingFilePath, Path.Combine(workspaceMappedPath, item.GitPath), true);
			        Workstation.Current.EnsureUpdateWorkspaceInfoCache(_versionControl, _credential.UserName);
                  
				    workspace.PendAdd(new[]{Path.Combine(workspaceMappedPath, item.GitPath)},true,"",LockLevel.None);
			    }

			    foreach (var item in changedItems.Modified)
			    {
                    if (_filter != null && _filter.Count > 0 && !IsFilterItem(item))
                        continue;
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
	            {
                    if (_filter != null && _filter.Count > 0 && !IsFilterItem(item))
                        continue;
                    _logger.Info("adding file security");
                    AddFileSecurity(Path.Combine(workspaceMappedPath, item.GitPath), _credential.Domain + "\\" + _credential.UserName, FileSystemRights.Read | FileSystemRights.ReadAndExecute | FileSystemRights.Write | FileSystemRights.FullControl, AccessControlType.Allow);
                    _logger.Info("deleting files");
	                workspace.PendDelete(Path.Combine(workspaceMappedPath, item.GitPath));

                    //TODO deleting folder when there are no files
                    //_logger.Info("deleting folders");
                    //if (IsDirectoryEmpty(Path.GetDirectoryName(filePath)))
                    //{
                    //    workspace.PendDelete(Path.GetDirectoryName(filePath));
                    //}
	            }
	        }
            catch (Exception ex)
            {
                _logger.Info("Error while commiting "+ex);
                _logger.Info(ex);
                throw;
            }
		}

        /// <summary>
        /// If filter items are configured then user can checkin only against these items and
        /// for each filtered item there is a corresponding TFS item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsFilterItem(Models.WorkspaceItem item)
        {
            bool isFilterItemConfigured = false;
            if (item != null && _filter != null && _filter.Count > 0)
            {
                foreach (FilterDetail filterItem in _filter)
                {
                    if (filterItem.GitSource.Equals(Path.GetDirectoryName(item.GitPath),
                                                                     StringComparison.CurrentCultureIgnoreCase))
                    {
                        _logger.Info("item is getting filtered " + filterItem.GitSource);
                        _logger.Info("item is getting filtered " + filterItem.TfsTarget);
                        item.GitPath = item.GitPath.Replace(filterItem.GitSource, filterItem.TfsTarget);
                        isFilterItemConfigured = true;
                        break;
                    }
                }
            }
            return isFilterItemConfigured;
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
            _logger.Info("_teamProjectPath:" + _teamProjectPath);
            _logger.Info("mappedFolderPath:" + mappedFolderPath);
			return workspace;

		}

		private void EnsureWorkspaceIsNotStale(VersionControlServer versionControl, string name)
		{
			try
			{
				var existingWorkspace = versionControl.GetWorkspace(name, versionControl.AuthorizedUser);
                _logger.Info("workspace name in EnsureWorkspaceIsNotStale:" + name);
				// if it got here, it means the workspace exists
				existingWorkspace.Delete();
                _logger.Info("workspace deleted");
			}
			catch (WorkspaceNotFoundException)
			{
                _logger.Info("WorkspaceNotFoundException");
				// do nothing
				// could have been much better if the API allowed some query to check for existing workspace
			}
		}
	}
}