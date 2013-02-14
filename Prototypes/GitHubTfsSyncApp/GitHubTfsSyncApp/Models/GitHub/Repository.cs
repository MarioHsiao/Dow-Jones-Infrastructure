using System;

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


		public DateTime CreatedAt { get; set; }
		public bool Fork { get; set; }
		public bool HasDownloads { get; set; }
		public bool HasIssues { get; set; }
		public bool HasWiki { get; set; }
		public int Id { get; set; }
		public int OpenIssues { get; set; }
		public DateTime PushedAt { get; set; }
		public int Size { get; set; }
		public int Stargazers { get; set; }
	}
}