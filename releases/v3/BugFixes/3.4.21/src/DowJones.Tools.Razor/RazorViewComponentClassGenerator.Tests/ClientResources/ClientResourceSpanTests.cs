using System.Web.Razor.Text;
using DowJones.Web.Razor;
using DowJones.Web.Razor.ClientResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.Razor.ClientResources
{
    [TestClass]
    public class ClientResourceSpanTests
    {
        const string RootNamespace = "Test.Test";
        const string RelativeResourceName = "Test.html";
        static readonly string ExpectedResourceName = string.Format("{0}.{1}", RootNamespace, RelativeResourceName);

        [TestMethod]
        public void ShouldGenerateFullResourceNameFromRelativeResourceNameWhenFullResourceNameIsNotSpecified()
        {
            var span = new TestClientResourceSpan("RelativeResourceName=" + RelativeResourceName)
                        { RootResourceNamespace = RootNamespace };

            Assert.AreEqual(ExpectedResourceName, span.ResourceName);
        }

        [TestMethod]
        public void ShouldGenerateFullResourceNameFromSingletonValue()
        {
            var span = new TestClientResourceSpan(RelativeResourceName) 
                        { RootResourceNamespace = RootNamespace };

            Assert.AreEqual(ExpectedResourceName, span.ResourceName);
        }


        public class TestClientResourceSpan : ClientResourceSpan 
        {
            public TestClientResourceSpan(string content)
                : base(new SourceLocation(), content)
            {
            }

            public override string AttributeTypeName
            {
                get { return "TEST"; }
            }
        }
    }
}
