using System.IO;
using System.Text;
using System.Xml;
using JsXmlDocParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Documentation.Tests.Tools.JsXmlDocParser
{
	[TestClass]
	public class JsParserTests
	{
        private JsParser _parser;

	    [TestInitialize]
        public void TestInitialize()
	    {
	        _parser = new JsParser();
	    }

	    [TestMethod]
		public void ShouldParseJsXmlCommentsInNamedFunctions()
		{
			const string comments = @"function jQuery(selector, context) {
											///	<summary>
											///  1: $(expression, context) - This function accepts a string containing a CSS selector which is then used to match a set of elements.
											///  2: $(html) - Create DOM elements on-the-fly from the provided String of raw HTML.
											///     3: $(elements) - Wrap jQuery functionality around a single or multiple DOM Element(s).
											///     4: $(callback) - A shorthand for $(document).ready().
											///     5: $() - As of jQuery 1.4, if you pass no arguments in to the jQuery() method, an empty jQuery set will be returned.
											///	</summary>
											///	       <param name=""selector"" type=""String"">
											///     1: expression - An expression to search with.
											///     2: html - A string of HTML to create on the fly.
											///     3: elements - DOM element(s) to be encapsulated by a jQuery object.
											///     4: callback - The function to execute when the DOM is ready.
											///	</param>
											///	<param name=""context"" type=""jQuery"">
											///     1: context - A DOM Element, Document or jQuery to use as context.
											///	</param>
											///	    <returns type=""jQuery"" />

											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										}";

			var results = _parser.Parse(new StringReader(comments), "in memory test");
            Assert.Inconclusive("Test assertion required");
		}

		[TestMethod]
		[DeploymentItem("Tools\\JsXmlDocParser\\SampleTestFiles\\jquery-1.6.2-vsdoc.js")]
		public void ShouldParseJQueryVsDoc()
		{
			string results;
			using (var reader = new StreamReader("jquery-1.6.2-vsdoc.js"))
			{
				results = _parser.Parse(reader, "jquery-1.6.2-vsdoc.js");
			}
			Assert.Inconclusive("Test assertion required");
		}

		[TestMethod]
		[DeploymentItem("Tools\\JsXmlDocParser\\SampleTestFiles\\jquery.validate-vsdoc.js")]
		public void ShouldParseJQueryValidateVsDoc()
		{
			string results;
			using (var reader = new StreamReader("jquery.validate-vsdoc.js"))
			{
				results = _parser.Parse(reader, "jquery.validate-vsdoc.js");
			}
            Assert.Inconclusive("Test assertion required");
		}
	}

    public static class JsParserExtensions
    {
        public static string Parse(this JsParser parser, TextReader reader, string assemblyName = null)
        {
            var sb = new StringBuilder();
            using (var writer = new MemberInfoWriter(XmlWriter.Create(sb)))
                parser.Parse(reader, writer);

            return "Awesome";
        }
    }
}
