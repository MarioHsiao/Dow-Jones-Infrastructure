using System.Collections.Generic;

namespace GitHubTfsSyncApp.Providers
{
	public class TreeCollection
	{
		public List<TreeNode> Tree { get; set; }
		public string Sha { get; set; }
		public string Url { get; set; }
	}
}