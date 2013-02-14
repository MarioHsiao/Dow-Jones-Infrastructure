using System;
using System.Collections.Generic;

namespace GitHubTfsSyncApp.Models.GitHub
{
	public class Commit
	{
		public string Id { get; set; }
		public string Url { get; set; }
		public Author Author { get; set; }
		public string Message { get; set; }
		public DateTime Timestamp { get; set; }
		public List<string> Added { get; set; }

		public Author Committer { get; set; }
		public bool Distinct { get; set; }
		public List<string> Modified { get; set; }
		public List<string> Removed { get; set; }
	}
}