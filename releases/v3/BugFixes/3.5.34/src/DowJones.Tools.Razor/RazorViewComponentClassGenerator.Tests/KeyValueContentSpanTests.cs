using System.Linq;
using System.Web.Razor.Text;
using DowJones.Web.Razor.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.Razor
{
    [TestClass]
    public class KeyValueContentSpanTests
    {
        [TestMethod]
        public void ShouldParseKeyValuePairs()
        {
            var span = new TestKeyValueContentSpan("KEY1=VALUE1, KEY2=VALUE2,KEY3=VALUE3");

            for(int i = 1; i <= 3; i++)
            {
                var actual = span.ContentKeyValuePairs.Single(x => x.Key == "KEY" + i).Value;
                Assert.AreEqual("VALUE" + i, actual);
            }
        }

        [TestMethod]
        public void ShouldParseSingletonValueWhenNoKeyValuePairsAreFound()
        {
            const string singletonValue = "SINGLETON";

            var span = new TestKeyValueContentSpan(singletonValue);

            Assert.AreEqual(singletonValue, span.SingletonValue);
        }

        [TestMethod]
        public void ShouldNotParseSingletonValueWhenKeyValuePairsAreFound()
        {
            var span = new TestKeyValueContentSpan("KEY=VALUE");

            Assert.IsNull(span.SingletonValue);
        }


        public class TestKeyValueContentSpan : KeyValueContentSpan
        {
            public TestKeyValueContentSpan(string content)
                : base(new SourceLocation(), content)
            {
            }
        }
    }
}
