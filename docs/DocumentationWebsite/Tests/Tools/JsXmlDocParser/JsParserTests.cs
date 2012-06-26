using System.Collections.Generic;
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

		[TestMethod, Ignore]
		public void CanParsePortalHeadlineJsDoc()
		{
			IEnumerable<IBlock> results;
            using (var streamReader = LoadResourceStreamReader("SampleTestFiles.PortalHeadlineList.vsdoc.js"))
			{
				var reader = new JavaScriptReaderAdapter(streamReader);
				_parser.Parse(reader);
				results = _parser.ParseResults.ToList();
			}

			Assert.AreEqual(1, results.Count());
			Assert.AreEqual(1, results.First().Children.Count(b=>b.BlockStarter.PatternType == PatternType.Options));
		}
	}
}
