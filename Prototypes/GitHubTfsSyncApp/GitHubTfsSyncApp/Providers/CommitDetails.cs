using System.Collections.Generic;
using GitHubTfsSyncApp.Models.GitHub;

namespace GitHubTfsSyncApp.Providers
{
	public class CommitDetails
	{
		public Author Committer { get; set; }
		public List<GitPointer> Parents { get; set; }
		public string Message { get; set; }
		public string Sha { get; set; }
		public GitPointer Tree { get; set; }
		public string Url { get; set; }
		public Author Author { get; set; }
	}
}