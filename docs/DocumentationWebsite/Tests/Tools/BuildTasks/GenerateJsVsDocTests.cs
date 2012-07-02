using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DowJones.Documentation.BuildTasks;

namespace DowJones.Documentation.Tests.Tools.BuildTasks
{
	[TestClass]
    public class GenerateJsVsDocTests
	{
		[TestMethod]
		[DeploymentItem("Tools\\BuildTasks\\TestFolder", "TestFolder")]
		public void ShouldProcessDirectory()
		{
		    var path = Path.Combine(Directory.GetCurrentDirectory(), "TestFolder");
		    var jsFilenames = Directory.GetFiles(path, "*.js");
		    var outputFilename = Path.Combine(Directory.GetCurrentDirectory(), "Output.vsdoc.xml");

		    var task = new ConvertJsDocToVsDoc
			{
				BuildEngine = new Mock<IBuildEngine>().Object,
                Filename = outputFilename,
                JsDocFiles = jsFilenames.Select(x => new TaskItem(x)).ToArray(),
            };

			Assert.IsTrue(task.Execute());
		}
	}
}
