using System;
using System.Linq;
using AttributeRouting.Helpers;
using GitHubTfsSyncApp.Models.GitHub;

namespace GitHubTfsSyncApp.Models
{
	public class CommitSpec : Commit
	{
		private readonly Lazy<string> _summary;

		private const string Template = "Message: {0}\n\n"
										+ "{1} additions, {2} modifications, {3} deletions\n"
										+ "Timestamp: {4}\n"
										+ "Author: {5}\n"
										+ "Committer: {6}\n"
										+ "Url: {7}";

		public CommitSpec(Commit commit)
		{
			// files in subfolders will be named "subfolder/filename", make them windows friendly paths  
			Added = commit.Added.Select(x => x.Replace("/", "\\")).ToList();
			Author = commit.Author;
			Committer = commit.Committer;
			Distinct = commit.Distinct;
			Id = commit.Id;
			Message = commit.Message;
			Modified = commit.Modified.Select(x => x.Replace("/", "\\")).ToList();
			Removed = commit.Removed.Select(x => x.Replace("/", "\\")).ToList();
			Timestamp = commit.Timestamp;
			Url = commit.Url;
			_summary = new Lazy<string>(() => Template.FormatWith(Message,
																  Added.Count(),
																  Modified.Count(),
																  Removed.Count(),
																  Timestamp.ToString("o"),
																  Author.Email,
																  Committer.Email,
																  Url));
		}

		public string LocalFilePath { get; set; }

		public string Summary { get { return _summary.Value; } }
	}
}