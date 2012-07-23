using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using DowJones.Infrastructure;
using DowJones.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using log4net;
using log4net.Util;

namespace DowJones.Web.Mvc.Infrastructure
{
	[TestClass]
	public class CoreJsHandlerTests : UnitTestFixture
	{
		[TestMethod]
		public void ShouldRenderCoreJsScripts()
		{
			//const string Url = "http://scripts.dev.us.factiva.com/core.js";

			//string actual;

			//using (var outputStream = new MemoryStream())
			//{
			//	var context = new Mock<HttpContextBase>();
			//	context.Setup(x => x.Request.Url).Returns(new Uri(Url));
			//	context.Setup(x => x.Request.Headers).Returns(new NameValueCollection());
			//	context.Setup(x => x.Items).Returns(EmptyDictionary.Instance);

			//	var response = new Mock<HttpResponseBase>();
			//	response.SetupGet(r => r.Output.Encoding).Returns(Encoding.ASCII);
			//	response.SetupGet(r => r.Filter).Returns(outputStream);

			//	context.Setup(x => x.Response).Returns(response.Object);

			//	var clientResourceManager = new Mock<IClientResourceManager>();

			//	clientResourceManager.Setup(r => r.GetClientResources(It.Is<IEnumerable<string>>(x => x.Contains("jquery")))).Returns(new List<ClientResource> { new ClientResource() { Name = "jquery"}});

			//	var handler = new CoreJsHandler()
			//		{
			//			ContentCache = new Mock<IContentCache>().Object,
			//			Log = new Mock<ILog>().Object,
			//			ClientResourceManager = clientResourceManager.Object,
			//			ClientResourceProcessors = Enumerable.Empty<IClientResourceProcessor>()
			//		};

			//	handler.OnProcessRequest(context.Object);

			//	actual = Encoding.ASCII.GetString(outputStream.ToArray());
			//}

			//var expected = "";

			//Assert.AreEqual(expected, actual);

			//Assert.Inconclusive("Come back and finish this after caching in ClientResources has been refactored");
		}
	}
}
