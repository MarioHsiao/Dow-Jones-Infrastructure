using DowJones.Factiva.Currents.ServiceModels;
using DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Summary.Packages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Factiva.Currents.Tests.ServiceModels
{
	[TestClass]
	public class TypeNameResolverTests
	{
		[TestMethod]
		public void CanResolveRelativeNameToType()
		{
			const string relativeName = "summaryTrendingPackage";
			var expectedType = typeof(SummaryTrendingPackage);
			var actualType = TypeNameResolver.ResolveNameToType(new[] { expectedType.Assembly }, relativeName);

			Assert.AreEqual(expectedType, actualType);
		}
	}
}
