using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsXmlDocParser.Tests
{
	[TestClass]
	public class FunctionInfoTests
	{
	    [TestMethod]
		public void ShouldParseNamedFunction()
		{
			const string jsBlock = @"function jQuery(selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										}";

			var result = new MemberInfo(jsBlock);
			Assert.AreEqual(result.Name, "jQuery");
		}

		[TestMethod]
		public void ShouldParseVariableNamedFunction()
		{
			const string jsBlock = @"var jQuery = function (selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										}";

			var result = new MemberInfo(jsBlock);
			Assert.AreEqual(result.Name, "jQuery");
		}

		[TestMethod]
		public void ShouldParseJsonVariableNamedFunction()
		{
			const string jsBlock = @"jQuery: function (selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										}";

			var result = new MemberInfo(jsBlock);
			Assert.AreEqual(result.Name, "jQuery");
		}

		[TestMethod]
		public void ShouldParseAnonymousFunction()
		{
			const string jsBlock = @"$.each(el, function (selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										});";

			var result = new MemberInfo(jsBlock);
            Assert.AreEqual(result.Name, MemberInfo.Anonymous);
		}

	    [TestMethod]
		public void ShouldExtractSignatureForNamedFunction()
		{
			const string jsBlock = @"function jQuery (selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										}";

			var result = new MemberInfo(jsBlock);
			Assert.AreEqual(result.Signature, "jQuery(selector, context)");
		}

		[TestMethod]
		public void ShouldExtractSignatureForVariableNamedFunction()
		{
			const string jsBlock = @"var jQuery= function (selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										}";

			var result = new MemberInfo(jsBlock);
			Assert.AreEqual(result.Signature, "jQuery(selector, context)");
		}

		[TestMethod]
		public void ShouldExtractSignatureForJsonVariableNamedFunction()
		{
			const string jsBlock = @"jQuery: function (selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										}";

			var result = new MemberInfo(jsBlock);
			Assert.AreEqual(result.Signature, "jQuery(selector, context)");
		}

		[TestMethod]
		public void ShouldExtractSignatureForAnonymousFunction()
		{
			const string jsBlock = @"$.each(el, function (selector, context) {
											// The jQuery object is actually just the init constructor 'enhanced'
											return new jQuery.fn.init(selector, context);
										});";

			var result = new MemberInfo(jsBlock);
			Assert.AreEqual(result.Signature, MemberInfo.Anonymous+"(selector, context)");
		}
	}
}