using System.IO;
using DowJones.Factiva.Currents.ServiceModels;
using DowJones.Factiva.Currents.ServiceModels.PageService;

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
			var searchAssemblies = new[] {typeof (PageServiceResponse).Assembly, typeof (DowJones.Pages.Tag).Assembly};

			var response = JsonConvert.DeserializeObject<PageServiceResponse>(rawResponse, new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.Auto,
					Binder = new TypeNameSerializationBinder(searchAssemblies),
				});

			Assert.IsInstanceOfType(response, typeof(PageServiceResponse));
		}
	}
}
