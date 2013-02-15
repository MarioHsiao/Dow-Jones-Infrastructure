using System;

namespace GitHubTfsSyncApp.Providers
{
	public static class UriExtensions
	{
		public static string GetBaseUrl(this Uri uri)
		{
			return uri.Scheme + "://" + uri.Host;
		}
	}
}