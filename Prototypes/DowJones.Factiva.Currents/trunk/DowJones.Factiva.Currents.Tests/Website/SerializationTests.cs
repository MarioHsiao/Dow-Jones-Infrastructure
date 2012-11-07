using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using DowJones.Factiva.Currents.Website.Models.PageService;

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
			var rawResponse = File.ReadAllText(fileName);

			var response = JsonConvert.DeserializeObject<PageServiceResponse>(rawResponse, new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.Auto,
				});

			Assert.IsInstanceOfType(response, typeof(PageServiceResponse));
		}
	}
}
