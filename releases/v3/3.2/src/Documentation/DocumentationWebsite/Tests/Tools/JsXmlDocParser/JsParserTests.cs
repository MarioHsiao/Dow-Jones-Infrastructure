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
		public void CanParseABunchOfFunctions()
	    {
	        const string source = @"
                DJ.UI.PortalHeadlineList = DJ.UI.PortalHeadlineList.extend({
                    /// <summary>
                    /// Portal Headline List Component.
                    /// </summary>

                    _initializeElements: function (ctx) {
                        /// <summary>
                        /// Initializes the jQuery element handles.
                        /// </summary>
                        /// <param name='ctx' mayBeNull='false' type='DOM element or jQuery selector context'>DOM Element or jQuery <a href='http://api.jquery.com/jquery/#selector-context'>selector context</a>.</param>
                        /// <code>
                        ///     this.$selectAll = ctx.find(this.selectors.selectAll);
                        ///     this.$options = ctx.find(this.selectors.headlineSelectOptions);
                        /// </code>
                        /// <remarks>
                        /// By doing the jQuery lookup once and memoizing the result, the script performance
                        /// is improved greatly.
                        /// 
                        /// If you're leveraging the framework, ctx is passed automatically during initialization of the component.
                        ///
                        /// This function is called automatically during initialization by the base class.
                        /// </remarks>
                    },

                    _initializeEventHandlers: function () {
                        /// <summary>
                        /// Initializes the event handlers for events like headline click.
                        /// </summary>
                        /// <remarks>
                        /// This function is called automatically during initialization by the base class.
                        /// </remarks>
                    },

                    _renderSnippets: function (headline, tLi) {
                        /// <summary>
                        /// Renders the Snippet for each headline and initializes the tooltip plugin to display snippets as a tooltip.
                        /// </summary>
                        /// <param name='headline'>The headline data.</param>
                        /// <param name='tLi'>DOM handle for the &lt;li&gt; item of the headline. </param>
                    },

                    bindOnSuccess: function (data) {
                        /// <summary>
                        /// Binds the on success template with the given data.
                        /// </summary>
                        /// <param name='data'>The headline data.</param>
                    }
                });
                ";

            var result = _parser.Parse(new StringReader(source)).First() as ClassBlock;

            // Assert functions
	        var functions = result.Children.OfType<FunctionBlock>();
            Debug.WriteLine(string.Format("Functions: {0}", string.Join(", ", functions.Select(x => x.Name))));
            CollectionAssert.AreEquivalent(
                new[] { "_initializeElements", "_initializeEventHandlers", "_renderSnippets", "bindOnSuccess" }, 
                functions.Select(x => x.Name).ToArray());
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
            CollectionAssert.AreEquivalent(
                new[] {
                    "init", "_initializeHeadlineList", "_initializeElements",
                    "_initializeEventHandlers", "_renderSnippets", 
                    "bindOnSuccess", "bindOnError", "showEditSection",
                    "getNoDataTemplate", "setNoDataTemplate",
                    "getErrorTemplate", "setErrorTemplate"
	            }, 
                functions.Select(x => x.Name).ToArray());
		}
	}
}
