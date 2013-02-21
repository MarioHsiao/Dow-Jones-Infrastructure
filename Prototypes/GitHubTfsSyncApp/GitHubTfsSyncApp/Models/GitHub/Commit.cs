using System;
using System.Collections.Generic;
using System.Linq;
using AttributeRouting.Helpers;

namespace GitHubTfsSyncApp.Models.GitHub
{
	public class Commit
	{
		private const string Template = "Message: {0}\n\n"
		                                + "{1} additions, {2} modifications, {3} deletions\n"
		                                + "Timestamp: {4}\n"
		                                + "Author: {5}\n"
		                                + "Committer: {6}\n"
		                                + "Url: {7}";

		private readonly Lazy<string> _summary;

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

		public Commit()
		{
			_summary = new Lazy<string>(() => Template.FormatWith(Message,
																  Added.Count(),
																  Modified.Count(),
																  Removed.Count(),
																  Timestamp.ToString("o"),
																  Author.Email,
																  Committer.Email,
																  Url));
		}

		public string Summary { get { return _summary.Value; } }
	}
}