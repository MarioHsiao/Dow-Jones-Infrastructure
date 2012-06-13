using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsXmlDocParser.Tests
{
	[TestClass]
	public class FunctionInfoTests
	{
		#region Parsing Function Name Tests

		[TestMethod]
		public void ShouldParseNamedFunction()
		{
			const string jsBlock = @"function jQuery(selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										}";

			var result = new FunctionInfo(jsBlock);
			Assert.AreEqual(result.Name, "jQuery");
		}

		[TestMethod]
		public void ShouldParseVariableNamedFunction()
		{
			const string jsBlock = @"var jQuery = function (selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										}";

			var result = new FunctionInfo(jsBlock);
			Assert.AreEqual(result.Name, "jQuery");
		}

		[TestMethod]
		public void ShouldParseJsonVariableNamedFunction()
		{
			const string jsBlock = @"jQuery: function (selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										}";

			var result = new FunctionInfo(jsBlock);
			Assert.AreEqual(result.Name, "jQuery");
		}

		[TestMethod]
		public void ShouldParseAnonymousFunction()
		{
			const string jsBlock = @"$.each(el, function (selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										});";

			var result = new FunctionInfo(jsBlock);
			Assert.AreEqual(result.Name, "anonymous");
		} 

		#endregion

		#region Parsing Function Signature Tests

		[TestMethod]
		public void ShouldExtractSignatureForNamedFunction()
		{
			const string jsBlock = @"function jQuery (selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										}";

			var result = new FunctionInfo(jsBlock);
			Assert.AreEqual(result.Signature, "jQuery(selector, context)");
		}

		[TestMethod]
		public void ShouldExtractSignatureForVariableNamedFunction()
		{
			const string jsBlock = @"var jQuery= function (selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										}";

			var result = new FunctionInfo(jsBlock);
			Assert.AreEqual(result.Signature, "jQuery(selector, context)");
		}

		[TestMethod]
		public void ShouldExtractSignatureForJsonVariableNamedFunction()
		{
			const string jsBlock = @"jQuery: function (selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										}";

			var result = new FunctionInfo(jsBlock);
			Assert.AreEqual(result.Signature, "jQuery(selector, context)");
		}

		[TestMethod]
		public void ShouldExtractSignatureForAnonymousFunction()
		{
			const string jsBlock = @"$.each(el, function (selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										});";

			var result = new FunctionInfo(jsBlock);
			Assert.AreEqual(result.Signature, "anonymous(selector, context)");
		}

		#endregion
	}
}