using System.Collections.Generic;

namespace GitHubTfsSyncApp.Models
{
	public class ChangedItems
	{
		public IEnumerable<WorkspaceItem> Added { get; set; }
		public IEnumerable<WorkspaceItem> Modified { get; set; }
		public IEnumerable<WorkspaceItem> Deleted { get; set; }
	}

	public class WorkspaceItem
	{
		public string IncomingFilePath { get; set; }
		public string GitPath { get; set; }
	}
}