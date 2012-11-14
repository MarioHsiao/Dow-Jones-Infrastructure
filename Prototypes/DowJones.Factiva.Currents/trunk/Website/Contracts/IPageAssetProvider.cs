using System.Collections.Generic;
using DowJones.Factiva.Currents.ServiceModels.PageService;
using DowJones.Factiva.Currents.Website.Models;

namespace DowJones.Factiva.Currents.Website.Contracts
{
	public interface IPageAssetProvider
	{
		PageServiceResponse GetPageByName(string pageName);
		PageServiceResponse GetPageById(string pageId);
		IEnumerable<PageListModel> GetPages();
	}
}