using System.Collections.Generic;

namespace GitHubTfsSyncApp.Models.GitHub
{
	public class WebHookResponse
	{
		public string Before { get; set; }
		public Repository Repository { get; set; }
		public List<Commit> Commits { get; set; }
		public string After { get; set; }
		public string Ref { get; set; }
	}
}