using DowJones.Factiva.Currents.Website.Models;

namespace DowJones.Factiva.Currents.Website.Contracts
{
	public interface ISearchContext
	{
		Headlines GetHeadlines(string searchContext);
	}
}