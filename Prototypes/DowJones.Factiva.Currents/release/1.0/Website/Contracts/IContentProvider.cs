using DowJones.Ajax.PortalHeadlineList;
using DowJones.Factiva.Currents.ServiceModels.PageService;

namespace DowJones.Factiva.Currents.Website.Contracts
{
    public interface IContentProvider
	{
		PortalHeadlinesServiceResult GetHeadlines(string searchContext);

		PortalHeadlineInfo GetHeadlineByAccessionNumber(string accessionNumber);
	}
}