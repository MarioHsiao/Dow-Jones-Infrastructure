using System.IO;

namespace GitHubTfsSyncApp.Extensions
{
	public static class DirectoryInfoExtensions
	{
		public static void ForceDelete(this DirectoryInfo directoryInfo)
		{
			foreach (var info in directoryInfo.GetFileSystemInfos("*", SearchOption.AllDirectories))
			{
				info.Attributes = FileAttributes.Normal;
			}

			directoryInfo.Delete(true);
		}
	}
}