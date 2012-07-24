using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Documentation.Tests.Core.Model
{
    [TestClass]
    public class NameTests
    {
        [TestMethod]
        public void ShouldInsertWhitespaceIntoDisplayName()
        {
            Assert.AreEqual(
                "My Awesome Name", 
                new Name("MyAwesomeName").DisplayName);
        }

        [TestMethod]
        public void ShouldInsertWhitespaceIntoDisplayNameWithNumbers()
        {
            Assert.AreEqual(
                "News Radar 90-Day Avg",
                new Name("NewsRadar90-DayAvg").DisplayName);
        }

        [TestMethod]
        public void ShouldLowercasePrepositions()
        {
            Assert.AreEqual(
                "The Fox and a Hound Go to and from the Store",
                new Name("TheFoxAndAHoundGoToAndFromTheStore").DisplayName);
        }

        [TestMethod]
        public void ShouldEquateToAnotherNameWithTheSameValue()
        {
            Assert.AreEqual(
                new Name("MyAwesomeName"), 
                new Name("MyAwesomeName"));
        }

        [TestMethod]
        public void ShouldNotEquateToAnotherNameWithADifferentValue()
        {
            Assert.IsTrue(new Name("MyAwesomeName1") != new Name("MyAwesomeName"));
        }

        [TestMethod]
        public void ShouldNotBeValidWhenValueIsNull()
        {
            Assert.IsFalse(new Name(null).IsValid());
        }

        [TestMethod]
        public void ShouldNotBeValidWhenValueIsWhitespace()
        {
            Assert.IsFalse(new Name("  ").IsValid());
        }

		[TestMethod]
		public void ShouldNotSplitKnownKeywords()
		{
			// known keywords: NuGet, JavaScript
			Assert.AreEqual("JavaScript Topic", new Name("JavaScriptTopic").DisplayName);
			Assert.AreEqual("NuGet Topic", new Name("NuGetTopic").DisplayName);
		}
    }
}