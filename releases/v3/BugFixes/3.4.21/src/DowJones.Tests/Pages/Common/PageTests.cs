using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using DowJones.DependencyInjection;
using DowJones.DependencyInjection.Ninject;
using DowJones.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GatewayPage = Factiva.Gateway.Messages.Assets.Pages.V1_0.NewsPage;

namespace DowJones.Pages
{
    [TestClass]
    public class PageTests : UnitTestFixture
    {
        [TestMethod]
        public void ShouldBeSerializeable()
        {
            InitializeServiceLocator();

            var page = new Page();
            using(var writer = new XmlTextWriter(new MemoryStream(), Encoding.Default))
                new DataContractSerializer(typeof(Page)).WriteObject(writer, page);
        }

        private static void InitializeServiceLocator()
        {
            var registry = AssemblyRegistry.CreateFromAssemblyReferences(Assembly.GetExecutingAssembly(), null);
            ServiceLocator.Initialize(new NinjectServiceLocatorFactory(registry));
        }
    }
}