using Microsoft.Build.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DowJones.Documentation.BuildTasks;

namespace DowJones.Documentation.Tests.Tools.BuildTasks
{
	[TestClass]
    public class GenerateJsVsDocTests
	{
		[TestMethod, Ignore]
		[DeploymentItem("Tools\\BuildTasks\\TestFolder", "TestFolder")]
		public void ShouldProcessDirectory()
		{
			const string dir = "TestFolder";
            var task = new ConvertJsDocToVsDoc
			{
				BuildEngine = new Mock<IBuildEngine>().Object
			};
			Assert.IsTrue(task.Execute());
		}
	}
}
