using System.Collections.Generic;
using DowJones.Factiva.Currents.Website.Contracts;
using DowJones.Web.Mvc.UI.Canvas;

namespace DowJones.Factiva.Currents.Website.Providers
{
	public class PageAssetProvider : IPageAssetProvider
	{
		#region Implementation of IPageAssetProvider

		public int MapPageNameToId(string name)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<IModule> GetModulesForPage(int pageId)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<IModule> GetModulesForPage(string pageName)
		{
			return GetModulesForPage(MapPageNameToId(pageName));
		}

		#endregion
	}
}