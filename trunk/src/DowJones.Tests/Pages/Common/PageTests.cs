using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using DowJones.DependencyInjection;
using DowJones.DependencyInjection.Ninject;
using DowJones.Infrastructure;
using DowJones.Infrastructure.Common;
using DowJones.Pages.Common;
using DowJones.Session;
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

        [TestMethod]
        public void PublishPage()
        {
            IControlData controlData = new ControlData();
            //controlData.UserID = "naresh4";
            //controlData.UserPassword = "passwd";
            //controlData.ProductID = "16";
            controlData.SessionID = "27139ZzZKJHEQT2CAAAGUAYAAAAAIFZJAAAAAABSGAYTGMBUGEYDEMJUGY2TAMBT";

            var pageAssetMgr = new PageAssetsManager(controlData, null, new Preferences.Preferences(), new Product("snapshot", ""));
            var list = new List<IShareAssets>();
            pageAssetMgr.PublishPage("28615", list);
           

        }

        [TestMethod]
        public void UnpublishPage()
        {
            IControlData controlData = new ControlData();
            //controlData.UserID = "naresh4";
            //controlData.UserPassword = "passwd";
            //controlData.ProductID = "16";
            controlData.SessionID = "27139ZzZKJHEQT2CAAAGUAYAAAAAIFZJAAAAAABSGAYTGMBUGEYDEMJUGY2TAMBT";

            var pageAssetMgr = new PageAssetsManager(controlData, null, new Preferences.Preferences(), new Product("snapshot", ""));
            var list = new List<IShareAssets>();
            pageAssetMgr.UnpublishPage("28615", list);

        }

    }
}