using DowJones.Factiva.Currents.Website.Models;

namespace DowJones.Factiva.Currents.Website.Contracts
{
    public interface IContentProvider
	{
		Headlines GetHeadlines(string searchContext);

        Headlines GetHeadlinesByAccessionNumber(string accessionNumber);
	}
}