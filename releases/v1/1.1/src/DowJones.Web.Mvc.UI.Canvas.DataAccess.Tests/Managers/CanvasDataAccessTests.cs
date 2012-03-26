// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasDataAccessTests.cs" company="">
//   
// </copyright>
// <summary>
//   Summary description for SyndicationNewsPageModuleDataManager
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SortBy = Factiva.Gateway.Messages.Assets.Pages.V1_0.SortBy;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Managers
{
    [TestClass]
    public class TestCanvasDataAccess : IntegrationTestFixture
    {
        [TestMethod, TestCategory("Integration")]
        public void ShouldGetNewsPagesListCollection()
        {
            var manager = new PageAssetsManager(new ControlData(), "en");
            
            Assert.IsTrue(manager.GetPageListInfoCollection(new List<PageType> {PageType.PNP}, SortOrder.Ascending, SortBy.Position).Count ==5 );
        }

//        [TestMethod, TestCategory("Integration")]
//        public void ShouldGetSubscribablePages()
//        {
//            var manager = new PageListManager(new ControlData(), "en");
//            var md = new Metadata();
//            md.IndustryCollection = new IndustryCollection() {"IACC"};
//            md.CategoryCollection = new CategoryCollection() {"iFIN"};
//            var pageListInfoExCollection = 
//                manager.GetSubscribablePages(PageType.NewsPage, 
//                SortOrder.Ascending, 
//                SortBy.Position, 
//                new List<AccessQualifier> {AccessQualifier.Factiva},Language.en,
//                new MetadataFilter() { 
//                    Filter=md,
//                    }
//                );
//            Assert.IsTrue(pageListInfoExCollection.Count>0);
//        }
    }
}
