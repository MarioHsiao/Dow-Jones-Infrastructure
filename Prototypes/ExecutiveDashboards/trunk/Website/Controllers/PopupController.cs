using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DowJones.Dash.Components.Model.StatsMap;
using DowJones.Dash.Modules;
using DowJones.Pages;
using DowJones.Pages.Modules;
using DowJones.Web.Mvc.UI.Canvas;
using DowJones.Web.Mvc.UI.Canvas.Controllers;
using DowJones.Web.Mvc.UI.Canvas.RavenDb;
using Raven.Client;
using Raven.Client.Embedded;
using Module = DowJones.Pages.Modules.Module;
using ZoneLayout = DowJones.Pages.Layout.ZoneLayout;

namespace DowJones.Dash.Website.Controllers
{
	public class PopupController : PagesControllerBase
	{

		[Authorize]
		public ActionResult Index(string pid, int mid)
		{
			var module = PageRepository.GetModule(pid, mid) as ScriptModule;
			var page = new Page
			{
				OwnerUserId = User.Identity.Name,
				ModuleCollection = new List<Module>(new[] { module }),
			};

			return Canvas(new Canvas { WebServiceBaseUrl = Url.Content("~/dashboard") }, page);
		}

		public ActionResult Demo()
		{
			var module = new PageLoadByRegionNewspageModule();

			var page = new Page
			{
				OwnerUserId = User.Identity.Name,
				ModuleCollection = new List<Module>(new[] { module }),
			};

			return Canvas(new Canvas { WebServiceBaseUrl = Url.Content("~/dashboard") }, page);
		}

	}
}
