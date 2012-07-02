using System.Diagnostics;
using System.IO;
using JsXmlDocParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DowJones.Documentation.Tests.Tools.JsXmlDocParser
{
	[TestClass]
    public class JsParserTests : TestClassWithEmbeddedResources
	{
		private JsParser _parser;

		[TestInitialize]
		public void TestInitialize()
		{
			_parser = new JsParser();
		}

	    [TestMethod]
	    public void ShouldParseJsDoc()
	    {
	        const string source =
                @"DJ.UI.SampleComponent = DJ.UI.Component.extend({
                    /// <summary>
                    /// Sample Component summary
                    /// </summary>
    
                    selectors: {
                    /// <summary>
                    /// Selectors field
                    /// </summary>
                    },

                    getData: function() {
                    /// <summary>
                    /// Function with no parameters
                    /// </summary>
                    },

                    init: function (element, meta) {
                        /// <summary>
                        /// Function with two parameters
                        /// </summary>
                        /// <param name='meta'>meta Parameter</param>
                        /// <param name='element'>element Parameter</param>
                    }
                });";

            var result = _parser.Parse(new StringReader(source)).First() as ClassBlock;

            Assert.AreEqual("DJ.UI.SampleComponent", result.FullName);
            
            Assert.AreEqual(3, result.Children.Count(),
                            "Wrong number of children");

            Assert.AreEqual("selectors", result.Children.OfType<FieldBlock>().Single().Name);

            Assert.AreEqual(2, result.Children.OfType<FunctionBlock>().Count(),
                            "Wrong number of functions");

            var initFunction = result.Child<FunctionBlock>("init");
            CollectionAssert.AreEquivalent(
                new[] { "element", "meta" }, 
                initFunction.Parameters.ToArray());
	    }


	    [TestMethod]
		public void CanParsePortalHeadlineJsDoc()
		{
            ClassBlock result;

            using (var reader = LoadResourceStreamReader("SampleTestFiles.PortalHeadlineList.vsdoc.js"))
			{
                result = _parser.Parse(reader).Single() as ClassBlock;
			}

            Assert.IsNotNull(result);

            // Assert fields
	        var fields = result.Children.OfType<FieldBlock>();
            Debug.WriteLine(string.Format("Fields: {0}", string.Join(", ", fields.Select(x => x.Name))));
	        CollectionAssert.AreEquivalent(
                new[] { "selectors", "options", "events" },
                fields.Select(x => x.Name).ToArray());

            // Assert functions
	        var functions = result.Children.OfType<FunctionBlock>();
            Debug.WriteLine(string.Format("Functions: {0}", string.Join(", ", functions.Select(x => x.Name))));
/*
            CollectionAssert.AreEquivalent(
                new[] {
                    "init", "_initializeHeadlineList", "_initializeElements",
                    "_renderSnippets", "bindOnError", "showEditSection",
                    "getNoDataTemplate", "setNoDataTemplate",
                    "getErrorTemplate", "setErrorTemplate"
	            }, 
                functions.Select(x => x.Name).ToArray());
*/
		}
	}
}
