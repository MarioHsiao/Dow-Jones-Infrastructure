using System.Collections.Generic;
using System.Linq;
using System.Web;
using DowJones.Ajax.PortalHeadlineList;

namespace DowJones.Factiva.Currents.Website.Contracts
{
	public interface ISearchContext
	{
		PortalHeadlineListDataResult GetHeadlines(string searchContext);
	}
}