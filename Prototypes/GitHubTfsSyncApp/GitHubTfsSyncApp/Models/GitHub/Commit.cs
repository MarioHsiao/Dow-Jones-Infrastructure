using System.Collections.Generic;

namespace GitHubTfsSyncApp.Models.GitHub
{
	public class Commit
	{
		public string Id { get; set; }
		public string Url { get; set; }
		public Author Author { get; set; }
		public string Message { get; set; }
		public string Timestamp { get; set; }
		public IEnumerable<string> Added { get; set; }
	}
}