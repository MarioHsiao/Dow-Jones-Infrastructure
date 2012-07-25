To test the newly created component. In project `DowJones.Web.Showcase`, create the following 2 files.

Under `Controllers` folder, create file:
#### SampleComponentController.cs

	using System.Web.Mvc;
	using DowJones.Web.Mvc.UI.Components.SampleComponent;

	namespace DowJones.Web.Showcase.Controllers
	{
		public class SampleComponentController : Controller
		{
			public ActionResult Index()
			{
				var model = new SampleComponentModel
				{
					TextColor = "red",
					TextSize = "20px",
					Data = new SampleComponentData
					{
						TextOne = "This is text one",
						TextTwo = "This is text two"
					}
				};

				return View(model);
			}
		}
	}

Under `Views` folder, create new folder `SampleComponent`.
Under newly created folder - `SampleComponent` - create file:
#### Index.cshtml

	@@using DowJones.Extensions
	@@{
		ViewBag.Title = "Sample Component";
	}

	@@Html.DJ().Render(Model)

After compiling and hosting the `DowJones.Web.Showcase` site, opening the following link

http://localhost/DowJones.Web.ShowCase/SampleComponent

Should give you the following:

	<div class="dj_SampleComponentContent" style="border: 1px solid black;">Text One:   
		<div style="color: red; font-size: 20px;" class="textOne">This is text one</div>
		<br>
		<br>
		Text Two:   
		<div style="color: red; font-size: 20px;" class="textTwo">This is text two</div>
	</div>