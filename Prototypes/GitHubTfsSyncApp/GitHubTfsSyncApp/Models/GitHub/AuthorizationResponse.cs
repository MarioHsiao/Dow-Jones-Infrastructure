using System;
using System.Collections.Generic;

namespace GitHubTfsSyncApp.Models.GitHub
{
	public class AuthorizationResponse
	{
		// restsharp needs this as List. Won't deserialize otherwise
		public List<string> Scopes { get; set; }
		public string Note { get; set; }
		public string Token { get; set; }
		
		public DateTime UpdatedAt { get; set; }
		
		public string Url { get; set; }

		public DateTime CreatedAt { get; set; }

		public App App { get; set; }

		public string NoteUrl { get; set; }

		public int Id { get; set; }
	}
}