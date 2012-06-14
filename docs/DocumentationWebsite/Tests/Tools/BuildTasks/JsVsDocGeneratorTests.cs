using System;
using DowJones.Documentation.BuildTasks;
using Microsoft.Build.Framework;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Documentation.Tests.Tools.BuildTasks
{
	[TestClass]
	public class JsVsDocGeneratorTests
	{
		[TestMethod]
		[DeploymentItem("Tools\\BuildTasks\\TestFolder", "TestFolder")]
		public void ShouldProcessDirectory()
		{
			const string dir = "TestFolder";
			var task = new JsVsDocGenerator
			{
				ProjectDir = dir,
				OutFile = "BuildTask.vsdoc.xml",
				BuildEngine = new Mock<IBuildEngine>().Object
			};
			Assert.IsTrue(task.Execute());
		}
	}
}
