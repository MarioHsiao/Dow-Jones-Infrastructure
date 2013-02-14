namespace GitHubTfsSyncApp.Models.GitHub
{
	public class Repository
	{
		public string Url { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Watchers { get; set; }
		public int Forks { get; set; }

		public int Private { get; set; }
		public Owner Owner { get; set; }
	}
}