using System.Collections.Generic;
using DowJones.Factiva.Currents.ServiceModels.PageService;
using DowJones.Web.Mvc.UI.Canvas;

namespace DowJones.Factiva.Currents.Website.Contracts
{
	public interface IPageAssetProvider
	{
		PageServiceResponse GetPageByName(string pageName);
		PageServiceResponse GetPageById(string pageId);
	}
}