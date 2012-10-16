using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nuget.RepositoryCleaner.Tests
{
	[TestClass]
	public class VersionTests
	{
		[TestMethod]
		public void CanParseFromMajorMinorStringToVersion()
		{
			const string majorMinor = "3.4";

			var actual = new Version(majorMinor);
			Assert.AreEqual(3, actual.Major);
			Assert.AreEqual(4, actual.Minor);
		}

		[TestMethod]
		public void CanParseFromMajorMinorBuildStringToVersion()
		{
			const string majorMinorBuild = "3.4.2";

			var actual = new Version(majorMinorBuild);
			Assert.AreEqual(3, actual.Major);
			Assert.AreEqual(4, actual.Minor);
			Assert.AreEqual(2, actual.Build);

		}

		[TestMethod]
		public void CanParseFromMajorMinorBuildRevisionStringToVersion()
		{
			const string majorMinorRevisionBuild = "3.4.5.1209";

			var actual = new Version(majorMinorRevisionBuild);
			Assert.AreEqual(3, actual.Major);
			Assert.AreEqual(4, actual.Minor);
			Assert.AreEqual(5, actual.Build);
			Assert.AreEqual(1209, actual.Revision);
		}

		[TestMethod]
		public void CanSortVersionsCorrectly()
		{
			var versionStrings = new[] { "3.3.1.174", "3.3.27", "3.3.9", "3.3.5", "3.3.0.1111", "2.0.116.41341" };

			var ascSortedVersions = new[]
				{
					new Version("2.0.116.41341"),
					new Version("3.3.0.1111"),
					new Version("3.3.1.174"),
					new Version("3.3.5"),
					new Version("3.3.9"),
					new Version("3.3.27"),
				};

			var actual = versionStrings.Select(s => new Version(s)).OrderBy(version => version).ToArray();

			for (var i = 0; i < actual.Count(); i++)
			{
				Assert.AreEqual(ascSortedVersions[i], actual[i]);
			}
			

		}
	}
}
