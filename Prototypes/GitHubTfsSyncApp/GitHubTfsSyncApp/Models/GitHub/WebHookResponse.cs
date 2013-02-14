using System.Collections.Generic;

namespace GitHubTfsSyncApp.Models.GitHub
{
	public class WebHookResponse
	{
		public string Before { get; set; }
		public string After { get; set; }
		public List<Commit> Commits { get; set; }

		public string Compare { get; set; }
		public bool Created { get; set; }
		public bool Deleted { get; set; }
		public bool Forced { get; set; }
		public Commit HeadCommit { get; set; }
		public string HookCallpath { get; set; }
		public Pusher Pusher { get; set; }
		public string Ref { get; set; }

		public Repository Repository { get; set; }
		
	}
}