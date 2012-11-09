using DowJones.Factiva.Currents.ServiceModels.PageService;

namespace DowJones.Factiva.Currents.Website.Contracts
{
	public interface IPageServiceResponseParser
	{
		PageServiceResponse Parse(string rawContent);
	}
}