namespace GitHubTfsSyncApp.Providers
{
	public class TreeNode
	{
		public string Type { get; set; }
		public string Sha { get; set; }
		public string Url { get; set; }
		public int Size { get; set; }
		public string Path { get; set; }
		public string Mode { get; set; }
	}
}