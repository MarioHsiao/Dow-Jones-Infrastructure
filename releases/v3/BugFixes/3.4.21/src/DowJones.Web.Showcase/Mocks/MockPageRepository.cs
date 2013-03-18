using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DowJones.Pages;
using DowJones.Pages.Common;
using DowJones.Pages.Modules;

namespace DowJones.Web.Showcase.Mocks
{
	public class MockPageRepository : IPageRepository
	{
		#region Implementation of IPageRepository

		public int AddModuleToPage(PageReference pageRef, Module module)
		{
			throw new NotImplementedException();
		}

		public void AddModuleToPage(PageReference pageRef, params int[] moduleIds)
		{
			throw new NotImplementedException();
		}

		public PageReference CreatePage(Page page)
		{
			throw new NotImplementedException();
		}

		public void DeletePage(PageReference pageRef)
		{
			throw new NotImplementedException();
		}

		public Module GetModule(PageReference pageRef, int moduleId)
		{
			throw new NotImplementedException();
		}

		public Page GetPage(PageReference pageRef)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Page> GetPages(SortBy sort, SortOrder order)
		{
			throw new NotImplementedException();
		}

		public void RemoveModulesFromPage(PageReference pageRef, params int[] moduleIds)
		{
			throw new NotImplementedException();
		}

		public void UpdateModule(Module module)
		{
			throw new NotImplementedException();
		}

		public void UpdatePage(Page page)
		{
			throw new NotImplementedException();
		}

		public void UpdatePageLayout(PageReference pageRef, PageLayout layout)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}