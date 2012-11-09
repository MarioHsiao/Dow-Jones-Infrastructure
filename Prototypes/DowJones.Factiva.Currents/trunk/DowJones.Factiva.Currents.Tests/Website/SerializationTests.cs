using System.IO;
using DowJones.Factiva.Currents.ServiceModels;
using DowJones.Factiva.Currents.ServiceModels.PageService;
using DowJones.Pages.Modules.Modules.Summary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DowJones.Factiva.Currents.Tests.Website
{
	[TestClass]
	public class SerializationTests
	{
		[TestMethod]
		[DeploymentItem(@"Website\TestData\PageServiceResponse.json", "TestData")]
		public void CanDeserializeGetPageByIdResponse()
		{
			const string fileName = @"TestData\PageServiceResponse.json";
			var rawResponse = File.ReadAllText(fileName).Replace("__type", "$type");
			var searchAssemblies = new[] {
				typeof (PageServiceResponse).Assembly, 
				typeof (Pages.Tag).Assembly,
 				typeof(SummaryNewspageModule).Assembly};

			var response = JsonConvert.DeserializeObject<PageServiceResponse>(rawResponse, new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.Auto,
					Binder = new TypeNameSerializationBinder(searchAssemblies),
				});

			Assert.IsInstanceOfType(response, typeof(PageServiceResponse));
		}

		
	}
}
