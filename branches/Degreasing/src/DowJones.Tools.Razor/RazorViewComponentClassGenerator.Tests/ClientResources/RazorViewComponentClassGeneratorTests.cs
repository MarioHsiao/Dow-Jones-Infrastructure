using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.Razor.ClientResources
{
    [TestClass]
    public class RazorViewComponentClassGeneratorTests : RazorViewComponentClassGeneratorTestFixture
    {

        [TestMethod]
        public void ShouldNotIncludeWebMatrixNamespacesInGeneratedCode()
        {
            const string template = @"Hello, World!";

            IEnumerable<string> classLines =
                GenerateCodeFromTemplateText(template)
                    .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => line.Trim());

            var webMatrixNamespaceCount = classLines.Count(line => line.Contains("System.Web.WebPages"));

            Assert.AreEqual(0, webMatrixNamespaceCount);
        }

    }
}
