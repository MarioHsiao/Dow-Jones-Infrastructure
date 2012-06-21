using System.Text;
using System.Xml;
using JsXmlDocParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Documentation.Tests.Tools.JsXmlDocParser
{
	[TestClass]
	public class JsDocToVsDocConverterTests
	{
		[TestMethod, Ignore]
		public void CanConvertEmbeddedJsResources()
		{
			var converter = new JsDocToVsDocConverter();
			var sb = new StringBuilder();
			using(var writer = XmlWriter.Create(sb, new XmlWriterSettings()
			{
				CloseOutput = true,
				Indent = true,
				IndentChars = "\t",
				OmitXmlDeclaration = true
			}))
			converter.Convert(GetType().Assembly, writer);

			
			Assert.IsNotNull(sb.ToString());
		}
	}
}
