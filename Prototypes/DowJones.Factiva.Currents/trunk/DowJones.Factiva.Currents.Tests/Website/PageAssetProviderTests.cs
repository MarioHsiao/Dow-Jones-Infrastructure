using System;
using DowJones.Factiva.Currents.ServiceModels.PageService;
using DowJones.Factiva.Currents.Website.Providers;
using DowJones.Pages.Modules.Modules.Summary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Factiva.Currents.Tests.Website
{
	[TestClass]
	public class PageAssetProviderTests
	{
		[TestMethod]
		public void CanGetPageById()
		{
			const string pageId = "Np_V1_26414_26414_000000";

			var provider = new PageAssetProvider(new PageServiceResponseParser(new[] { 
				typeof(PageServiceResponse).Assembly, 
				typeof(Pages.Tag).Assembly,
 				typeof(SummaryNewspageModule).Assembly 
			}));

			var page = provider.GetPageById(pageId);

			
			Assert.AreEqual(0, page.NewsPageServiceResult.ReturnCode);

			Assert.AreEqual("Accounting/Consulting", page.NewsPageServiceResult.Package.NewsPage.Title);
		}

		[TestMethod]
		public void CanGetPageByName()
		{
			const string pageName = "Accounting/Consulting";

			var provider = new PageAssetProvider(new PageServiceResponseParser(new[] { 
				typeof(PageServiceResponse).Assembly, 
				typeof(Pages.Tag).Assembly,
 				typeof(SummaryNewspageModule).Assembly 
			}));

			var page = provider.GetPageByName(pageName);

			Assert.AreEqual(0, page.NewsPageServiceResult.ReturnCode);

			Assert.AreEqual("Accounting/Consulting", page.NewsPageServiceResult.Package.NewsPage.Title);
		}
	}
}
